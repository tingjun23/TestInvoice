using System;
using System.Data.Entity.Migrations;
using System.Linq;
using TestInvoice.DataAccess.Entity;

namespace TestInvoice.DataAccess.EntityFramework.Seeds
{
    public static partial class Seed
    {
        public static void SeedGoodAndService(InvoiceDbContext context)
        {
            DateTime now = DateTime.UtcNow;

            if (!context.GoodAndService.Any(a => a.Name == "GoodAndService1"))
            {
                var goodAndService1 = new GoodAndService
                {
                    Name = "GoodAndService1",
                    UnitPrice = 10,
                };
                context.GoodAndService.AddOrUpdate(goodAndService1);
            }

            if (!context.GoodAndService.Any(a => a.Name == "GoodAndService2"))
            {
                var goodAndService2 = new GoodAndService
                {
                    Name = "GoodAndService2",
                    UnitPrice = 20,
                };
                context.GoodAndService.AddOrUpdate(goodAndService2);
            }

            if (!context.GoodAndService.Any(a => a.Name == "GoodAndService3"))
            {
                var goodAndService3 = new GoodAndService
                {
                    Name = "GoodAndService3",
                    UnitPrice = 30,
                };
                context.GoodAndService.AddOrUpdate(goodAndService3);
            }

            if (!context.GoodAndService.Any(a => a.Name == "GoodAndService4"))
            {
                var goodAndService4 = new GoodAndService
                {
                    Name = "GoodAndService4",
                    UnitPrice = 40,
                };
                context.GoodAndService.AddOrUpdate(goodAndService4);
            }
        }
    }
}
