using System.ComponentModel.DataAnnotations;

namespace Bravo.Supermarket.API.Dto.FromSql
{
    public class CARI_HESAPLAR
    {
        [Key]
        public string cari_kod { get; set; }
        public string cari_unvan1 { get; set; }
    }
}
