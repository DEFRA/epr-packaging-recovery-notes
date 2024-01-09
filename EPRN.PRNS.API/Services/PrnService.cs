using AutoMapper;
using EPRN.Common.Data.DataModels;
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

        public async Task<int> CreatePrnRecord(int materialId)
        {
            var prnRecord = new PackagingRecoveryNote
            {
                WasteTypeId = materialId
            };
            return await _prnRepository.CreatePrnRecord(prnRecord);
        }

        public async Task<double?> GetTonnage(int id)
        {
            return await _prnRepository.GetTonnage(id);
        }

        public async Task SaveTonnage(int id, double tonnage)
        {
            await _prnRepository.UpdateTonnage(id, tonnage);
        }
    }
}