namespace Bravo.Supermarket.API.Dto.Xml
{
    public class OrderLineDto
    {
        public int LineNumber { get; set; }
        public string EAN { get; set; }
        public string BuyerItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemType { get; set; }
        public double OrderedQuantity { get; set; }
        public double FreeOrderedQuantity { get; set; }
        public double OrderedUnitPacksize { get; set; }
        public string UnitOfMeasure { get; set; }
        public double OrderedUnitNetPrice { get; set; }
        public int ScheduledQuantity { get; set; }
    }
}
