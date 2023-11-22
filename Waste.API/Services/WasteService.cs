using AutoMapper;
using EPRN.Common.Dtos;
using Microsoft.EntityFrameworkCore;
using Waste.API.Models;
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

        public async Task<int> CreateJourney()
        {
            var journeyRecord = new WasteJourney();
            _wasteContext.WasteJourney.Add(journeyRecord);
            await _wasteContext.SaveChangesAsync();
            return journeyRecord.Id;
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            var journeyRecord = await _wasteContext.WasteJourney.FirstOrDefaultAsync(wj => wj.Id == journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.Month = selectedMonth;
            await _wasteContext.SaveChangesAsync();
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
