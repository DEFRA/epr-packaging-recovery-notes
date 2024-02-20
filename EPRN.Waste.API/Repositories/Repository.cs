using AutoMapper;
using EPRN.Common.Data;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Data.Enums;
using EPRN.Common.Dtos;
using EPRN.Waste.API.Repositories.Interfaces;
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

        public async Task UpdateJourneySiteId(int id, int siteId)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.SiteId = siteId;
                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task UpdateJourneyBaledWithWire(
            int id, 
            bool baledWithWire, 
            double baledWithWireDeductionPercentage)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.BaledWithWire = baledWithWire;
                waste.BaledWithWireDeductionPercentage = baledWithWireDeductionPercentage;
                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task UpdateJourneyTonnage(int id, double journeyTonnage)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.Tonnes = journeyTonnage;
                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task UpdateJourneyDoneId(int id, DoneWaste selectedDoneWaste)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.DoneWaste = selectedDoneWaste;
                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task UpdateJourneyWasteTypeId(int id, int wasteTypeId)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.WasteTypeId = wasteTypeId;
                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task UpdateJourneyMonth(int id, int selectedMonth)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.Month = selectedMonth;
                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task UpdateJourneySubTypeAndAdjustment(
            int id,
            int subTypeId,
            double adjustment)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.Adjustment = adjustment;
                waste.WasteSubTypeId = subTypeId;

                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task UpdateJourneyCategory(int id, Category category)
        {
            await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
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

        public async Task<string> GetWasteTypeName(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
                .Select(wj => wj.WasteType.Name)
                .SingleOrDefaultAsync();
        }

        public async Task<int?> GetWasteTypeId(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
                .Select(wj => wj.WasteTypeId)
                .SingleOrDefaultAsync();
        }

        public async Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
                .Select(wj => new WasteSubTypeSelectionDto
                {
                    WasteSubTypeId = wj.WasteSubTypeId.Value,
                    Adjustment = wj.WasteSubType.AdjustmentPercentageRequired ? wj.Adjustment : null
                })
                .SingleOrDefaultAsync();
        }
        public async Task<int?> GetSelectedMonth(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
                .Select(wj => wj.Month)
                .SingleOrDefaultAsync();
        }
        public async Task<DoneWaste?> GetDoneWaste(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
                .Select(wj => wj.DoneWaste)
                .SingleOrDefaultAsync();
        }

        public async Task<WasteTonnageDto> GetWasteTonnage(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
                .Select(wj => new WasteTonnageDto
                {
                    ExportTonnes = wj.Tonnes,
                    Id = id,
                    Category = _mapper.Map<Common.Enums.Category>(wj.Category)
                })
                .SingleOrDefaultAsync();
        }

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(w => w.Id == id)
                .Select(w => _mapper.Map<WasteRecordStatusDto>(w))
                .SingleOrDefaultAsync();
        }

        public async Task<BaledWithWireDto> GetBaledWithWire(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(w => w.Id == id)
                .Select(w => _mapper.Map<BaledWithWireDto>(w))
                .SingleOrDefaultAsync();
        }

        public async Task<WasteJourney> GetWasteJourneyById_FullModel(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Include(x => x.WasteType)
                .Include(x => x.WasteSubType)
                .Where(wj => wj.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<JourneyAnswersDto> GetWasteJourneyAnswersById(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Include(x => x.WasteType)
                .Include(x => x.WasteSubType)
                .Where(wj => wj.Id == id)
                .Select(x => _mapper.Map<JourneyAnswersDto>(x))
                .SingleOrDefaultAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var exists = await _wasteContext
                .WasteJourney
                .AnyAsync(wj => wj.Id == id);

            return exists;
        }

        public async Task<NoteDto> GetWasteNote(int id)
        {
            return await _wasteContext.WasteJourney
                .Where(wj => wj.Id == id)
                .Select(x => new NoteDto
                {
                    Id = id,
                    Note = x.Note,
                    WasteCategory = Enum.Parse<Common.Enums.Category>(x.Category.ToString())
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdateWasteNote(int id, string note)
        {
            var waste = await GetJourney(id);

            if (waste != null)
            {
                waste.Note = note;
                await _wasteContext.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == id)
                .Select(wj => wj.Category)
                .SingleOrDefaultAsync();
        }

        public async Task<double?> GetTotalQuantityForAllUserJourneys(string userReferenceId)
        {
            var totalQuantity = await _wasteContext
                .WasteJourney
                .Where(x => x.UserReference == userReferenceId)
                .SumAsync(x => x.Tonnes);

            return totalQuantity;
        }

        /// <summary>
        /// This method exists so we can perform an update on WasteJourney
        /// and record the auditing of the record using the original values
        /// </summary>
        private async Task<WasteJourney> GetJourney(int id)
        {
            return await _wasteContext.WasteJourney.FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
