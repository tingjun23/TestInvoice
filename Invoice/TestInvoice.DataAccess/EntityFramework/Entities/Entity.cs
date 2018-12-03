using System;
using System.ComponentModel.DataAnnotations;

namespace TestInvoice.DataAccess.EntityFramework.Entities
{
    public class Entity : IEntity
    {
        public bool IsDeleted { get; set; }
    }
}
