using TestInvoice.DataAccess.Entity;
using TestInvoice.DataAccess.EntityFramework;
using TestInvoice.DataAccess.Repositories.Interfaces;

namespace TestInvoice.DataAccess.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(InvoiceDbContext context) : base(context)
        {
        }
    }
}
