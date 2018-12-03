using System;

namespace TestInvoice.DataAccess.EntityFramework.Entities
{
    public interface IEntity
    {
        bool IsDeleted { get; set; }
    }
}
