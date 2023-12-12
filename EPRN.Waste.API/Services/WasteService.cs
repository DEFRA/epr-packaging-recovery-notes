using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Waste.API.Models;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services.Interfaces;

namespace EPRN.Waste.API.Services
{
    public class WasteService : IWasteService
    {
        public readonly IMapper _mapper;
        public readonly IRepository _wasteRepository;

        public WasteService(
            IMapper mapper,
            IRepository wasteRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _wasteRepository = wasteRepository ?? throw new ArgumentNullException(nameof(_wasteRepository));
        }

        public async Task<IEnumerable<WasteTypeDto>> WasteTypes()
        {
            _wasteRepository.LazyLoading = false;
            
            // we want the entire table contents (at the
            // moment - there may be more requirements in the future)
            // so no where clause
            var dbWasteTypes = await _wasteRepository
                .GetAllWasteTypes();

            return _mapper.Map<List<WasteTypeDto>>(dbWasteTypes);
        }

        public async Task<IEnumerable<WasteSubTypeDto>> WasteSubTypes(int wasteTypeid)
        {
            _wasteRepository.LazyLoading = false;
            await Task.CompletedTask;

            var dbWasteSubTypes = _wasteRepository
                .List<WasteSubType>()
                .Where(subType => subType.WasteTypeId == wasteTypeid)
                .OrderBy(subType => subType.Id)
                .ToList();

            return _mapper.Map<List<WasteSubTypeDto>>(dbWasteSubTypes);
        }
    }
}
