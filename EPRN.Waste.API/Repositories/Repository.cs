using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Waste.API.Data;
using EPRN.Waste.API.Models;
using EPRN.Waste.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPRN.Waste.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly WasteContext _wasteContext;

        public Repository(WasteContext wasteContext)
        {
            _wasteContext = wasteContext ?? throw new ArgumentNullException(nameof(wasteContext));
        }

        public async Task AddJourney(WasteJourney wasteJourney)
        {
            _wasteContext.Add(wasteJourney);
            await _wasteContext.SaveChangesAsync();
        }

        public async Task UpdateJourneySiteId(int journeyId, int siteId)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.SiteId, siteId)
                );
        }

        public async Task UpdateJourneyBaledWithWire(int journeyId, bool baledWithWire)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.BaledWithWire, baledWithWire)
                );
        }

        public async Task UpdateJourneyTonnage(int journeyId, double journeyTonnage)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.Tonnes, journeyTonnage)
                );
        }

        public async Task UpdateJourneyDoneId(int journeyId, DoneWaste selectedDoneWaste)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.DoneWaste, selectedDoneWaste)
                );
        }

        public async Task UpdateJourneyWasteTypeId(int journeyId, int wasteTypeId)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.WasteTypeId, wasteTypeId)
                );
        }

        public async Task UpdateJourneyMonth(int journeyId, int selectedMonth)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp => 
                    sp.SetProperty(wj => wj.Month, selectedMonth)
                );
        }

        public async Task UpdateJourneySubTypeAndAdjustment(
            int journeyId,
            int subTypeId,
            double adjustment)
        {
            var journeyRecord = new WasteJourney
            {
                Id = journeyId,
                Adjustment = adjustment,
                WasteSubTypeId = subTypeId,
            };

            _wasteContext.Attach(journeyRecord);
            _wasteContext.Entry(journeyRecord).Property("WasteSubTypeId").IsModified = true;
            _wasteContext.Entry(journeyRecord).Property("Adjustment").IsModified = true;
            await _wasteContext.SaveChangesAsync();
        }

        public async Task<IList<WasteType>> GetAllWasteTypes()
        {
            return await _wasteContext
                .WasteType
                .ToListAsync();
        }

        public async Task<IList<WasteSubType>> GetWasteSubTypes(int wasteTypeId)
        {
            return await _wasteContext
                .WasteSubType
                .Where(st => st.WasteTypeId == wasteTypeId)
                .ToListAsync();
        }

        public async Task<string> GetWasteTypeName(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.WasteType.Name)
                .SingleOrDefaultAsync();
        }

        public async Task<int?> GetWasteTypeId(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.WasteTypeId)
                .SingleOrDefaultAsync();
        }

        public async Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int journeyId)
        {
            var result = await _wasteContext
                .WasteJourney
                .Include(wj => wj.WasteSubType)
                .Where(wj => wj.Id == journeyId)
                .Select(wj => new WasteSubTypeSelectionDto
                {
                    WasteSubTypeId = wj.WasteSubType.Id,
                    Adjustment = wj.WasteSubType.AdjustmentPercentageRequired ? wj.Adjustment : null
                })
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task<DoneWaste?> GetDoneWaste(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.DoneWaste)
                .SingleOrDefaultAsync();
        }

        public async Task<double?> GetWasteTonnage(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.Tonnes)
                .SingleOrDefaultAsync();
        }

        public async Task<WasteJourney> GetWasteJourneyById(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> Exists(int journeyId)
        {
            var exists = await _wasteContext
                .WasteJourney
                .AnyAsync(wj => wj.Id == journeyId);

            return exists;
        }
    }
}
