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
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BarkodTanimlari>().ToTable("BARKOD_TANIMLARI");
        }
    }
}
