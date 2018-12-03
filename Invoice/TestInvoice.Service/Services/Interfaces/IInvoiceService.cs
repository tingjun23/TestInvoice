using System.Collections.Generic;
using System.Threading.Tasks;
using TestInvoice.Core.Dto;

namespace TestInvoice.Service.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDto>> GetInvoiceByIdAsync(int invoiceId);
        Task<List<InvoiceDto>> GetInvoicesAsync();
        Task<List<InvoiceDto>> CreateInvoiceAsync(List<InvoiceDto> invoices);
        Task<List<InvoiceDto>> UpdateInvoiceAsync(List<InvoiceDto> invoices);
    }
}
