namespace Bravo.Supermarket.API.Dto.ToSql
{
    public class SendingOrderDto
    {
        public string sip_stok_kod { get; set; }
        public double sip_miktar { get; set; }
        public string sip_satici_kod { get; set; }
        public double sip_b_fiyat { get; set; }
        public double sip_tutar { get; set; }
        public double sip_iskonto1 { get; set; }
        public double sip_depono { get; set; }
        public string sip_musteri_kod { get; set; }
        public string sip_aciklama { get; set; }
    }
}
