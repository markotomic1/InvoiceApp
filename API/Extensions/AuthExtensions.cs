

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;
using API.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer((options) =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async context =>
                        {
                            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                            {
                                var token = authorizationHeader.ToString().Split(" ")[1];
                                var httpClient = new HttpClient();

                                //dohvatatanje rute za jwks
                                var response =
                                    await httpClient.GetAsync("http://auth-server:8080/.well-known/openid-configuration");

                                response.EnsureSuccessStatusCode();

                                string responseBody = await response.Content.ReadAsStringAsync();
                                DiscoveryDocument discoveryDocument = JsonSerializer.Deserialize<DiscoveryDocument>(responseBody);


                                if (discoveryDocument != null && discoveryDocument.JwksUri != null)
                                {
                                    //dohvatanje jwks
                                    var jwksClient = new HttpClient();
                                    var jwksResponse = await jwksClient.GetAsync(discoveryDocument.JwksUri);

                                    jwksResponse.EnsureSuccessStatusCode();

                                    string jwksBody = await jwksResponse.Content.ReadAsStringAsync();
                                    List<JwkBody> jwkParsed = JsonSerializer.Deserialize<List<JwkBody>>(jwksBody);

                                    //pronalazak javnog kljuca
                                    var publicKey = "";

                                    foreach (var item in jwkParsed)
                                    {
                                        if (item.KeyId == ExtractKidFromToken(token))
                                        {
                                            publicKey = item.PublicKey;
                                        }
                                    }

                                    if (publicKey != "")
                                    {


                                        byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

                                        RSAParameters rsaParams = GetRsaParametersFromBase64(publicKeyBytes);



                                        var securityKey = new RsaSecurityKey(rsaParams);

                                        context.HttpContext.Request.Scheme = JwtBearerDefaults.AuthenticationScheme;

                                        //validacija tokena
                                        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                                        {
                                            ValidateIssuerSigningKey = true,
                                            IssuerSigningKey = securityKey,
                                            ValidateIssuer = false,
                                            ValidateAudience = false,
                                        };
                                        options.TokenValidationParameters = tokenValidationParameters;

                                    }

                                }
                            }
                        }
                };
            });

            services.AddAuthorization();

            return services;
        }

        private static RSAParameters GetRsaParametersFromBase64(byte[] publicKeyBytes)
        {

            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(publicKeyBytes, out _);
                return rsa.ExportParameters(false);
            }
        }
        private static string ExtractKidFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (jsonToken?.Header != null)
            {
                // provjeri kid
                if (jsonToken.Header.TryGetValue("kid", out var kid))
                {
                    return kid.ToString();
                }
            }


            return null;
        }

    }
}