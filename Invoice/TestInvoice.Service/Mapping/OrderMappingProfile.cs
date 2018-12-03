using AutoMapper;
using TestInvoice.Core.Dto;
using TestInvoice.DataAccess.Entity;

namespace TestInvoice.Service.Mapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
