using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Faktura> Fakture { get; set; }
        public DbSet<StavkaFakture> StavkeFakture { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faktura>()
                .HasIndex(f => f.Broj)
                .IsUnique();

        }
    }
}