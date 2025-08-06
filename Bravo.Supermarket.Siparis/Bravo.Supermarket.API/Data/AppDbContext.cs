using Bravo.Supermarket.API.Dto.FromSql;
using Microsoft.EntityFrameworkCore;

namespace Bravo.Supermarket.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BarkodTanimlari> barkodTanimlari { get; set; }
        public DbSet<CARI_HESAP_ADRESLERI> CARI_HESAP_ADRESLERI { get; set; }
        public DbSet<CARI_HESAPLAR> CARI_HESAPLAR { get; set; }
        public DbSet<SIPARISLER> SIPARISLER { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.58.7;Database=MikroDB_V15_ARAN_2024;User Id=sa;Password=P@ssword4;Encrypt=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BarkodTanimlari>().ToTable("BARKOD_TANIMLARI");
        }
    }
}
