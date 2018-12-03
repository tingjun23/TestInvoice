using System.Linq;
using System.Runtime.CompilerServices;

namespace TestInvoice.DataAccess.Repositories.Interfaces
{
    public interface IRepository<T> where T : EntityFramework.Entities.Entity
    {
        IQueryable<T> Get();
        T Add(T entity);
        void Update(T entity);
        int SaveChanges();
    }
}
