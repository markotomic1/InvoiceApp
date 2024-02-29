using AuthServer.Entities;
using AuthServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public void DodajKorisnika(Korisnik korisnik)
        {
            _context.Add(korisnik);
        }

        public Task<Korisnik> DohvatiKorisnika(string email)
        {
            return _context.Korisnici
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> SacuvajIzmjene()
        {
            return await _context.SaveChangesAsync() > 0;
        }



        public void DodajKljuc(UserKey userKey)
        {
            _context.UserKey.Add(userKey);
        }

        public async Task<List<UserKey>> DohvatiKljuceve()
        {
            return await _context.UserKey.ToListAsync();
        }

        public void SacuvajAuthCode(AuthCode authCode)
        {
            _context.AuthCodes.Add(authCode);
        }

        public async Task<AuthCode> DohvatiAuthCode(string code)
        {
            return await _context.AuthCodes.FirstOrDefaultAsync(x => x.CodeChallenge == code);
        }
    }
}