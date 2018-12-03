using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TestInvoice.Core.Dto;
using TestInvoice.DataAccess.Repositories.Interfaces;
using TestInvoice.Service.Services.Interfaces;

namespace TestInvoice.Service.Services
{
    public class ClientService : IClientService
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public ClientService(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<List<ClientDto>> GetClientsAsync()
        {
            try
            {
                var clients = await _clientRepository.Get().ToListAsync();
                return _mapper.Map<List<ClientDto>>(clients);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
