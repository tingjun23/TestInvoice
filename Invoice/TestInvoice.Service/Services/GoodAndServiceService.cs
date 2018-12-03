using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TestInvoice.Core.Dto;
using TestInvoice.DataAccess.Repositories.Interfaces;
using TestInvoice.Service.Services.Interfaces;

namespace TestInvoice.Service.Services
{

    public class GoodAndServiceService : IGoodAndServiceService
    {
        private readonly IMapper _mapper;
        private readonly IGoodAndServiceRepository _goodAndServiceRepository;

        public GoodAndServiceService(IMapper mapper, IGoodAndServiceRepository goodAndServiceRepository)
        {
            _mapper = mapper;
            _goodAndServiceRepository = goodAndServiceRepository;
        }

        public async Task<List<GoodAndServiceDto>> GetGoodAndServicesAsync()
        {
            try
            {
                var goodAndServices = await _goodAndServiceRepository.Get().ToListAsync();
                return _mapper.Map<List<GoodAndServiceDto>>(goodAndServices);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
