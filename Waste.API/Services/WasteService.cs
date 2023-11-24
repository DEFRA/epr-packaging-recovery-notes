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
            var journeyRecord = new WasteJourney
            {
                CreatedDate = DateTime.Now,
                CreatedBy = "DEVELOPER"
            };

            _wasteContext.WasteJourney.Add(journeyRecord);
            await _wasteContext.SaveChangesAsync();
            return journeyRecord.Id;
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            var journeyRecord = await _wasteContext.WasteJourney
                .FirstOrDefaultAsync(wj => wj.Id == journeyId);

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

        public async Task SaveWasteType(int journeyId, int wasteTypeId)
        {
            var journeyRecord = await _wasteContext.WasteJourney.FirstOrDefaultAsync(wj => wj.Id == journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.WasteTypeId = wasteTypeId;
            await _wasteContext.SaveChangesAsync();
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            var wasteType = await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId && wj.WasteTypeId != null)
                .Select(wj => wj.WasteType!.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(wasteType))
                throw new ArgumentNullException($"No waste type found for {journeyId}");

            return wasteType;
        }
    }
}
