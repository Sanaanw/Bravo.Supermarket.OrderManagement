using System.ComponentModel.DataAnnotations;

namespace Bravo.Supermarket.API.Dto.FromSql
{
    public class BarkodTanimlari
    {
        [Key]
        public string bar_stokkodu { get; set; }
        public string bar_kodu { get; set; }
    }
}
