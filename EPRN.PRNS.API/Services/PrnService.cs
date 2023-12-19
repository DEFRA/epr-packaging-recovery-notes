using AutoMapper;
using EPRN.PRNS.API.Configuration;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EPRN.PRNS.API.Services
{
    public class PrnService : IPrnService
    {
        public readonly IMapper _mapper;
        public readonly IRepository _prnRepository;

        public PrnService(
            IMapper mapper,
            IRepository wasteRepository,
            IOptions<AppConfigSettings> configSettings)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prnRepository = wasteRepository ?? throw new ArgumentNullException(nameof(_prnRepository));

        }

        public Task<int> CreatePrnRecord()
        {
            throw new NotImplementedException();
        }
    }
}