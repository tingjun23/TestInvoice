using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using TestInvoice.DataAccess.Repositories.Interfaces;

namespace TestInvoice.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityFramework.Entities.Entity
    {
        protected DbContext Context;
        protected readonly DbSet<T> Dbset;

        public Repository(DbContext context)
        {
            Context = context;
            Dbset = context.Set<T>();
        }

        public virtual IQueryable<T> Get()
        {
            return Dbset.Where(x => !x.IsDeleted);
        }

        public virtual T Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity) + " is null.");
            }

            return Dbset.Add(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity) + " is null.");
            }

            Dbset.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public int SaveChanges()
        {
            // Save changes with the default options
            return Context.SaveChanges();
        }
    }
}
