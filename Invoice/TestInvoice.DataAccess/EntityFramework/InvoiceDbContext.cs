using EntityFramework.DynamicFilters;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using TestInvoice.DataAccess.Entity;
using TestInvoice.DataAccess.EntityFramework.Entities;

namespace TestInvoice.DataAccess.EntityFramework
{
    public class InvoiceDbContext : DbContext
    {
        public DbSet<Client> Client { get; set; }
        public DbSet<GoodAndService> GoodAndService { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<BulkInsertSession> BulkInsertSession { get; set; }

        public InvoiceDbContext() : base("name=InvoiceDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.HasDefaultSchema(Schemas.InvoiceSchemas);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Properties<string>().Configure(a => a.HasMaxLength(1000));
            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);
            modelBuilder.Filter("IsDeleted", (Entities.Entity a) => a.IsDeleted, false);
        }
    }
}
