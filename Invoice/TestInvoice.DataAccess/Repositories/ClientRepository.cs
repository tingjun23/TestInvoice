using TestInvoice.DataAccess.Entity;
using TestInvoice.DataAccess.EntityFramework;
using TestInvoice.DataAccess.Repositories.Interfaces;

namespace TestInvoice.DataAccess.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(InvoiceDbContext context) : base(context)
        {
        }
    }
}
