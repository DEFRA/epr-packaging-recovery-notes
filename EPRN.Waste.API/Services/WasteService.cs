using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services.Interfaces;

namespace EPRN.Waste.API.Services
{
    public class WasteService : IWasteService
    {
        public readonly IRepository _wasteRepository;

        public WasteService(
            IRepository wasteRepository)
        {
            _wasteRepository = wasteRepository ?? throw new ArgumentNullException(nameof(_wasteRepository));
        }

        public async Task<Dictionary<int, string>> WasteTypes()
        {
            // we want the entire table contents (at the
            // moment - there may be more requirements in the future)
            // so no where clause
            return new List<WasteTypeDto>(await _wasteRepository
                .GetAllWasteTypes())
                .ToDictionary(wt => wt.Id, t => t.Name);
        }

        public async Task<IEnumerable<WasteSubTypeDto>> WasteSubTypes(int wasteTypeid)
        {
            return await _wasteRepository
                .GetWasteSubTypes(wasteTypeid);
        }
    }
}
