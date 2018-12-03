namespace TestInvoice.Core.Dto
{
    public class OrderDto 
    {
        public int Id { get; set; }
        public int GoodAndServiceId { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }

        public GoodAndServiceDto GoodAndService { get; set; }
    }
}
