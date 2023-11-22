using AutoMapper;
using EPRN.Common.Dtos;
using Microsoft.EntityFrameworkCore;
using Waste.API.Services.Interfaces;
using WasteManagement.API.Data;

namespace Waste.API.Services
{
    public class WasteService : IWasteService
    {
        public readonly IMapper _mapper;
        public readonly WasteContext _wasteContext;

        public WasteService(
            IMapper mapper,
            WasteContext wasteContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _wasteContext = wasteContext ?? throw new ArgumentNullException(nameof(wasteContext));
        }

        public async Task<IEnumerable<WasteTypeDto>> WasteTypes()
        {
            return await _wasteContext.WasteType
                .OrderBy(wt => wt.Name)
                .Select(wt => _mapper.Map<WasteTypeDto>(wt))
                .ToListAsync();
        }
    }
}
