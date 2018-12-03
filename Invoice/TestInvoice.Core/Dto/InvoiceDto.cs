using System;
using System.Collections.Generic;

namespace TestInvoice.Core.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime InvoiceDate { get; set; }

        public ClientDto Client { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
