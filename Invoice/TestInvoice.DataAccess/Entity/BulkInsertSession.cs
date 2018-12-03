using System;

namespace TestInvoice.DataAccess.Entity
{
    public class BulkInsertSession : EntityFramework.Entities.Entity
    {
        public int Id { get; set; }
        public TimeSpan CreatedTimeStamp { get; set; }
    }
}
