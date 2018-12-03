namespace TestInvoice.DataAccess.Entity
{
    public class Order : EntityFramework.Entities.Entity
    {
        public int Id { get; set; }
        public int GoodAndServiceId { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }

        public virtual GoodAndService GoodAndService { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
