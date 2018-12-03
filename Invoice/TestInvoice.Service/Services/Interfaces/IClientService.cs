using System.Collections.Generic;
using System.Threading.Tasks;
using TestInvoice.Core.Dto;

namespace TestInvoice.Service.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetClientsAsync();
    }
}
