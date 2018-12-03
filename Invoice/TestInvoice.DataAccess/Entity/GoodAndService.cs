namespace TestInvoice.DataAccess.Entity
{
    public class GoodAndService : EntityFramework.Entities.Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
