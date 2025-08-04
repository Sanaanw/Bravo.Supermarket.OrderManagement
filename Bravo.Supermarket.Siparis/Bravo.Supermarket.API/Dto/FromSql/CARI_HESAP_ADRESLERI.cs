using System.ComponentModel.DataAnnotations;

namespace Bravo.Supermarket.API.Dto.FromSql
{
    public class CARI_HESAP_ADRESLERI
    {
        [Key]
        public string adr_temsilci_kodu { get; set; }
        public string adr_cari_kod  { get; set; }
    }
}
