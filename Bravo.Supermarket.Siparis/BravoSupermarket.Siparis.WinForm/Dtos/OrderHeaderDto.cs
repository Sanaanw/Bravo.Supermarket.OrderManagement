namespace BravoSupermarket.Siparis.WinForm.Dtos
{
    public class OrderHeaderDto
    {
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string OrderCurrency { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string AdditionalRemarks { get; set; }
        public string TestInformation { get; set; }
        public string ContractNumber { get; set; }
        public string PaymentTerms { get; set; }
    }

}
