using System.Collections.Generic;

namespace BravoSupermarket.Siparis.WinForm.Dtos
{
    public class OrderDto
    {
        public OrderHeaderDto Header { get; set; }
        public OrderPartyDto Parties { get; set; }
        public List<OrderLineDto> Lines { get; set; }
        public int TotalLines { get; set; }
    }
}
