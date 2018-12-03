using TestInvoice.DataAccess.Entity;
using TestInvoice.DataAccess.EntityFramework;
using TestInvoice.DataAccess.Repositories.Interfaces;

namespace TestInvoice.DataAccess.Repositories
{
    public class GoodAndServiceRepository : Repository<GoodAndService>, IGoodAndServiceRepository
    {
        public GoodAndServiceRepository(InvoiceDbContext context) : base(context)
        {
        }
    }
}
