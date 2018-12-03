namespace TestInvoice.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<EntityFramework.InvoiceDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EntityFramework.InvoiceDbContext context)
        {
            EntityFramework.Seeds.Seed.SeedClient(context);
            context.SaveChanges();
            EntityFramework.Seeds.Seed.SeedGoodAndService(context);
            context.SaveChanges();

        }
    }
}
