using AuthServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<UserKey> UserKey { get; set; }
        public DbSet<AuthCode> AuthCodes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}