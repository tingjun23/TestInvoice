using AutoMapper;
using TestInvoice.Core.Dto;
using TestInvoice.DataAccess.Entity;

namespace TestInvoice.Service.Mapping
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
