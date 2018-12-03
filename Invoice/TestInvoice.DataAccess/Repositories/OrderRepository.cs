using TestInvoice.DataAccess.Entity;
using TestInvoice.DataAccess.EntityFramework;
using TestInvoice.DataAccess.Repositories.Interfaces;

namespace TestInvoice.DataAccess.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(InvoiceDbContext context) : base(context)
        {
        }
    }
}
