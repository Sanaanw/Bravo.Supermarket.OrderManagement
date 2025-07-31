namespace Bravo.Supermarket.API.Dto.Xml
{
    public class OrderDto
    {
        public OrderHeaderDto Header { get; set; }
        public OrderPartyDto Parties { get; set; }
        public List<OrderLineDto> Lines { get; set; }
        public int TotalLines { get; set; }
    }
}
