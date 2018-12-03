using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TestInvoice.Core.Dto;
using TestInvoice.Service.Services.Interfaces;

namespace TestInvoice.WebApi.Controllers
{
    public class InvoiceController : ApiController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<List<InvoiceDto>> GetInvoices(int? invoiceId = null)
        {
            if (invoiceId == null || invoiceId.Value == 0)
                return await _invoiceService.GetInvoicesAsync();
            return await _invoiceService.GetInvoiceByIdAsync(invoiceId.Value);
        }

        [HttpPost]
        public async Task<List<InvoiceDto>> CreateInvoice(List<InvoiceDto> invoices)
        {
            if (invoices == null)
                throw new Exception("Invalid invoice.");
            return await _invoiceService.CreateInvoiceAsync(invoices);
        }

        [HttpPut]
        public async Task<List<InvoiceDto>> UpdateInvoice(List<InvoiceDto> invoices)
        {
            if (invoices == null)
                throw new Exception("Invalid invoice.");
            return await _invoiceService.UpdateInvoiceAsync(invoices);
        }
    }
}