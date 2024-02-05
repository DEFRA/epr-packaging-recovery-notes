using AutoMapper;
using EPRN.Common.Data;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Data.Enums;
using EPRN.Common.Dtos;
using EPRN.Waste.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace EPRN.Waste.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly IMapper _mapper;
        private readonly EPRNContext _wasteContext;

        public Repository(
            IMapper mapper,
            EPRNContext eprnContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _wasteContext = eprnContext ?? throw new ArgumentNullException(nameof(eprnContext));
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

        public async Task UpdateJourneyBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp => sp
                    .SetProperty(wj => wj.BaledWithWire, baledWithWire)
                    .SetProperty(p => p.BaledWithWireDeductionPercentage, baledWithWireDeductionPercentage)
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

        public async Task UpdateJourneyCategory(int journeyId, Category category)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.Category, category)
                );
        }

        public async Task<IList<WasteTypeDto>> GetAllWasteTypes()
        {
            return await _wasteContext
                .WasteType
                .Select(wt => _mapper.Map<WasteTypeDto>(wt))
                .ToListAsync();
        }

        public async Task<IList<WasteSubTypeDto>> GetWasteSubTypes(int wasteTypeId)
        {
            return await _wasteContext
                .WasteSubType
                .Where(st => st.WasteTypeId == wasteTypeId)
                .Select(wst => _mapper.Map<WasteSubTypeDto>(wst))
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
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => new WasteSubTypeSelectionDto
                {
                    WasteSubTypeId = wj.WasteSubTypeId.Value,
                    Adjustment = wj.WasteSubType.AdjustmentPercentageRequired ? wj.Adjustment : null
                })
                .SingleOrDefaultAsync();
        }
        public async Task<int?> GetSelectedMonth(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.Month)
                .SingleOrDefaultAsync();
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

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(w => w.Id == journeyId)
                .Select(w => _mapper.Map<WasteRecordStatusDto>(w))
                .SingleOrDefaultAsync();
        }

        public async Task<BaledWithWireDto> GetBaledWithWire(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(w => w.Id == journeyId)
                .Select(w => _mapper.Map<BaledWithWireDto>(w))
                .SingleOrDefaultAsync();
        }

        public async Task<WasteJourney> GetWasteJourneyById_FullModel(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Include(x => x.WasteType)
                .Include(x => x.WasteSubType)
                .Where(wj => wj.Id == journeyId)
                .SingleOrDefaultAsync();
        }

        public async Task<JourneyAnswersDto> GetWasteJourneyAnswersById(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Include(x => x.WasteType)
                .Include(x => x.WasteSubType)
                .Where(wj => wj.Id == journeyId)
                .Select(x => _mapper.Map<JourneyAnswersDto>(x))
                .SingleOrDefaultAsync();
        }

        public async Task<bool> Exists(int journeyId)
        {
            var exists = await _wasteContext
                .WasteJourney
                .AnyAsync(wj => wj.Id == journeyId);

            return exists;
        }

        public async Task<NoteDto> GetWasteNote(int journeyId)
        {
            return await _wasteContext.WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(x => new NoteDto
                {
                    JourneyId = journeyId,
                    Note = x.Note,
                    WasteCategory = Enum.Parse<Common.Enums.Category>(x.Category.ToString())
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdateWasteNote(int journeyId, string note)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.Note, note)
                );
        }

        public async Task<Category> GetCategory(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.Category)
                .SingleOrDefaultAsync();
        }
    }
}
