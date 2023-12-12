using EPRN.Common.Enums;
using EPRN.Waste.API.Data;
using EPRN.Waste.API.Models;
using EPRN.Waste.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPRN.Waste.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly WasteContext _wasteContext;

        public Repository(WasteContext wasteContext)
        {
            _wasteContext = wasteContext ?? throw new ArgumentNullException(nameof(wasteContext));
        }
        //public async Task<int> Add<T>(T entity)
        //    where T : IdBaseEntity
        //{
        //    _wasteContext.Add(entity);
        //    await _wasteContext.SaveChangesAsync();

        //    return entity.Id;
        //}

        //public async Task Delete<T>(T entity)
        //    where T : IdBaseEntity
        //{
        //    _wasteContext.Remove(entity);
        //    await _wasteContext.SaveChangesAsync();
        //}

        //public async Task<T> GetById<T>(int id)
        //    where T : IdBaseEntity
        //{
        //    return await _wasteContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        //}

        //public IEnumerable<T> List<T>()
        //    where T : IdBaseEntity
        //{
        //    return _wasteContext
        //        .Set<T>()
        //        .AsQueryable();
        //}

        //public IEnumerable<T> List<T>(Expression<Func<T, bool>> predicate) 
        //    where T : IdBaseEntity
        //{
        //    return _wasteContext
        //        .Set<T>()
        //        .Where(predicate)
        //        .AsEnumerable();
        //}

        //public async Task Update<T>(T entity) 
        //    where T : IdBaseEntity
        //{
        //    _wasteContext.Entry(entity).State = EntityState.Modified;
        //    await _wasteContext.SaveChangesAsync();
        //}



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

        public async Task<IList<WasteType>> GetAllWasteTypes()
        {
            return await _wasteContext
                .WasteType
                .ToListAsync();
        }

        public async Task<string> GetWasteTypeName(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.WasteType.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetWasteTypeId(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.WasteTypeId)
                .FirstOrDefaultAsync();
        }

        public async Task<DoneWaste?> GetDoneWaste(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.DoneWaste)
                .FirstOrDefaultAsync();
        }

        public async Task<double?> GetWasteTonnage(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .Select(wj => wj.Tonnes)
                .FirstOrDefaultAsync();
        }

        public async Task<WasteJourney> GetWasteJourneyById(int journeyId)
        {
            return await _wasteContext
                .WasteJourney
                .Where(wj => wj.Id == journeyId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Exists(int journeyId)
        {
            var exists = await _wasteContext
                .WasteJourney
                .AnyAsync(wj => wj.Id == journeyId);

            return exists;
        }

        public bool LazyLoading
        {
            set
            {
                _wasteContext.ChangeTracker.LazyLoadingEnabled = value;
            }
        }
    }
}
