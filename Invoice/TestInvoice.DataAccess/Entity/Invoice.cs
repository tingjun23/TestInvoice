using System;
using System.Collections.Generic;

namespace TestInvoice.DataAccess.Entity
{
    public class Invoice : EntityFramework.Entities.Entity
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int? BulkInsertSessionId { get; set; }

        public virtual Client Client { get; set; }
        public List<Order> Orders { get; set; }
    }
}
