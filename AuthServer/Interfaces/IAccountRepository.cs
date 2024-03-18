using AuthServer.Entities;

namespace AuthServer.Interfaces
{
    public interface IAccountRepository
    {
        public void DodajKorisnika(Korisnik korisnik);
        public Task<bool> SacuvajIzmjene();
        public Task<Korisnik> DohvatiKorisnika(string email);
        public void DodajKljuc(UserKey userKey);
        public Task<List<UserKey>> DohvatiKljuceve();
        public void SacuvajAuthCode(AuthCode authCode);
        public Task<AuthCode> DohvatiAuthCode(string code);
        public void AddRefreshToken(RefreshToken refreshToken);
        public Task<RefreshToken> GetRefreshToken(string refreshToken);
    }
}