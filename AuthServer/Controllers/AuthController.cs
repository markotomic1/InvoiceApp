using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using AuthServer.DTOs;
using AuthServer.Entities;
using AuthServer.Helpers;
using AuthServer.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        public AuthController(IAccountRepository accountRepository, IMapper mapper, ITokenService tokenService, IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _tokenService = tokenService;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }



        [HttpPost("register")]
        public async Task<ActionResult<string>> Registracija(RegisterDto registerDto, [FromQuery] string returnUrl)
        {
            if (await _accountRepository.DohvatiKorisnika(registerDto.Email)
            != null)
                return BadRequest("Registracija nije moguca");

            if (registerDto.Lozinka != registerDto.PotvrdaLozinke)
                return BadRequest("Nisu uneseni ispravni podaci");

            var korisnik = _mapper.Map<Korisnik>(registerDto);

            using var hmac = new HMACSHA512();

            korisnik.Ime = registerDto.Ime.ToLower();
            korisnik.LozinkaHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(registerDto.Lozinka));
            korisnik.LozinkaSalt = hmac.Key;

            _accountRepository.DodajKorisnika(korisnik);

            await _accountRepository.SacuvajIzmjene();

            var token = _tokenService.CreateToken(korisnik);

            return Ok(new { url = returnUrl, token });

        }


        [HttpPost("login")]
        public async Task<ActionResult> Prijava(LoginDto loginDto, [FromQuery] string returnUrl)
        {

            var korisnik = await _accountRepository
                .DohvatiKorisnika(loginDto.Email);

            if (korisnik == null) return Unauthorized();

            using var hmac = new HMACSHA512(korisnik.LozinkaSalt);

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Lozinka));

            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != korisnik.LozinkaHash[i])
                    return BadRequest();
            }
            await _accountRepository.SacuvajIzmjene();



            var token = _tokenService.CreateToken(korisnik);

            return Ok(new { url = returnUrl, token });
        }

        [Authorize]
        [HttpGet("authorize")]
        public async Task<ActionResult> Authorize(AuthorizationDto authorizationDto)
        {

            var protector = _dataProtectionProvider.CreateProtector("oauth");
            var code = _mapper.Map<AuthCodeDto>(authorizationDto);
            code.Expiry = DateTime.Now.AddMinutes(5);

            var authCode = _mapper.Map<AuthCode>(code);
            _accountRepository.SacuvajAuthCode(authCode);

            if (!await _accountRepository.SacuvajIzmjene())
            {
                return BadRequest();
            }

            var codeString = protector.Protect(JsonSerializer.Serialize(code));
            var redirectUrl = $"{authorizationDto.RedirectUri}?code={codeString}&state={authorizationDto.State}&iss={HttpUtility.UrlEncode("http://auth-server:5001")}";


            return Ok(new { url = redirectUrl });
        }

        [HttpPost("token")]
        public async Task<ActionResult> Token([FromForm] IFormCollection form)
        {
            if (form["grant_type"] == "refresh_token")
            {
                //generisanje refresh tokena
            }

            var protector = _dataProtectionProvider.CreateProtector("oauth");

            var codeString = protector.Unprotect(form["code"]);
            var authCodeForm = JsonSerializer.Deserialize<AuthCodeDto>(codeString);

            var codeVerifier = DohvatiCodeVerifier(form["code_verifier"]);

            if (codeVerifier != authCodeForm.CodeChallenge)
            {
                return Forbid();
            }
            var authCode = await _accountRepository.DohvatiAuthCode(codeVerifier);

            if (authCode.Used == true || DateTime.UtcNow > authCode.Expiry)
                return Forbid();

            authCode.Used = true;
            await _accountRepository.SacuvajIzmjene();

            var tokenPublicKeyModel = GenerateAccessTokenAndPublicKey();

            _accountRepository.DodajKljuc(tokenPublicKeyModel.PublicKey);

            var refreshToken = GenerateRefreshToken();


            await _accountRepository.SacuvajIzmjene();

            return Ok(new { access_token = tokenPublicKeyModel.AccessToken, token_type = "Bearer", refreshToken });
        }


        [HttpGet(".well-known/jwks.json")]
        public async Task<ActionResult<List<UserKey>>> DohvatiJwk()
        {
            var userKeys = await _accountRepository.DohvatiKljuceve();
            if (userKeys == null) return BadRequest();

            return Ok(userKeys);
        }

        private string DohvatiCodeVerifier(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var codeChallenge = Base64UrlEncoder.Encode(sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier)));

            return codeChallenge;
        }

        private static string GetKeyIdFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwt) as JwtSecurityToken;

            return jsonToken?.Header?.Kid;
        }
        private static TokenPublicKeyModel GenerateAccessTokenAndPublicKey()
        {
            var rsaKey = RSA.Create();

            var keyToSign = new RsaSecurityKey(rsaKey);
            keyToSign.KeyId = Guid.NewGuid().ToString();


            var handler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Claims = new Dictionary<string, object>()
                {
                    [JwtRegisteredClaimNames.Sub] = Guid.NewGuid().ToString(),
                    ["role"] = "AuthorizedUser"
                },
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials =
                      new SigningCredentials(keyToSign, SecurityAlgorithms.RsaSha256)
            };
            var access_token = handler.CreateToken(tokenDescriptor);

            var publicKey = new UserKey
            {
                KeyId = GetKeyIdFromJwt(handler.WriteToken(access_token)),
                PublicKey = Convert.ToBase64String(rsaKey.ExportRSAPublicKey())
            };

            var response = new TokenPublicKeyModel
            {
                AccessToken = handler.WriteToken(access_token),
                PublicKey = publicKey,
            };

            return response;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

    }
}