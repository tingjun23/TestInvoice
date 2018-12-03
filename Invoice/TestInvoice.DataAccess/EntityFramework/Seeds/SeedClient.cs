using System;
using System.Data.Entity.Migrations;
using System.Linq;
using TestInvoice.DataAccess.Entity;

namespace TestInvoice.DataAccess.EntityFramework.Seeds
{
    public static partial class Seed
    {
        public static void SeedClient(InvoiceDbContext context)
        {
            DateTime now = DateTime.UtcNow;

            if (!context.Client.Any(a => a.Name == "Client1"))
            {
                var client1 = new Client
                {
                    Name = "Client1",
                    Company = "Company1",
                    Address = "Address1",
                    City = "City1",
                    State = "State1",
                    Phone = "1111111",
                    Email = "email1@email.com",
                    Website = "website1.com.my",
                };
                context.Client.AddOrUpdate(client1);
            }

            if (!context.Client.Any(a => a.Name == "Client2"))
            {
                var client2 = new Client
                {
                    Name = "Client2",
                    Company = "Company2",
                    Address = "Address2",
                    City = "City2",
                    State = "State2",
                    Phone = "2222222",
                    Email = "email2@email.com",
                    Website = "website2.com.my",
                };
                context.Client.AddOrUpdate(client2);
            }

            if (!context.Client.Any(a => a.Name == "Client3"))
            {
                var client3 = new Client
                {
                    Name = "Client3",
                    Company = "Company3",
                    Address = "Address3",
                    City = "City3",
                    State = "State3",
                    Phone = "3333333",
                    Email = "email3@email.com",
                    Website = "website3.com.my",
                };
                context.Client.AddOrUpdate(client3);
            }

            if (!context.Client.Any(a => a.Name == "Client4"))
            {
                var client4 = new Client
                {
                    Name = "Client4",
                    Company = "Company4",
                    Address = "Address4",
                    City = "City4",
                    State = "State4",
                    Phone = "4444444",
                    Email = "email4@email.com",
                    Website = "website4.com.my",
                };
                context.Client.AddOrUpdate(client4);
            }

        }
    }
}
