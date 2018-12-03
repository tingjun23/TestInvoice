using AutoMapper;
using TestInvoice.Core.Dto;
using TestInvoice.DataAccess.Entity;

namespace TestInvoice.Service.Mapping
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile()
        {
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
        }
    }
}
