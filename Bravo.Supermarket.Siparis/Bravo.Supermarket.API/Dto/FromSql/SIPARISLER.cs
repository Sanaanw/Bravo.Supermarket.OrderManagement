using System.ComponentModel.DataAnnotations;

namespace Bravo.Supermarket.API.Dto.FromSql
{
    public class SIPARISLER
    {
        [Key]
        public string sip_sto_kod { get; set; }
        public decimal sip_miktar { get; set; }
        public string satici_kodu { get; set; }
        public decimal sip_bb_fiyat { get; set; }
        public decimal sip_tutar { get; set; }
        public decimal sip_iskonto1 { get; set; }
        public int sip_depono { get; set; }
        public string sip_musteri_kod { get; set; }
    }
}
