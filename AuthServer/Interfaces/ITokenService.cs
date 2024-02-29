using AuthServer.Entities;

namespace AuthServer.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Korisnik korisnik);
    }
}