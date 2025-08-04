namespace Bravo.Supermarket.API.Dto.ToSql
{
    public class InsertDto
    {
        public List<SendingOrderDto> SendingOrders { get; set; } = new();
    }
}
