using AutoMapper;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services.Interfaces;

namespace EPRN.PRNS.API.Services
{
    public class PrnService : IPrnService
    {
        public readonly IMapper _mapper;
        public readonly IRepository _prnRepository;

        public PrnService(
            IMapper mapper,
            IRepository prnRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prnRepository = prnRepository ?? throw new ArgumentNullException(nameof(prnRepository));
        }

        public async Task<int> CreatePrnRecord()
        {
            return await _prnRepository.CreatePrnRecord();
        }

        public async Task<int?> GetTonnage(int id)
        {
            return await _prnRepository.GetTonnage(id);
        }

        public async Task SaveTonnage(int id, int tonnage)
        {
            await _prnRepository.UpdateTonnage(id, tonnage);
        }
    }
}