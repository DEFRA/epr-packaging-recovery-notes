using EPRN.Common.Enums;
using EPRN.Waste.API.Models;
using System.Linq.Expressions;

namespace EPRN.Waste.API.Repositories.Interfaces
{
    public interface IRepository
    {
        //Task<T> GetById<T>(int id) where T : IdBaseEntity;

        //IEnumerable<T> List<T>() where T : IdBaseEntity;

        //IEnumerable<T> List<T>(Expression<Func<T, bool>> predicate) where T : IdBaseEntity;

        //Task<int> Add<T>(T entity) where T : IdBaseEntity;

        //Task Delete<T>(T entity) where T : IdBaseEntity;

        //Task Update<T>(T entity) where T : IdBaseEntity;

        bool LazyLoading { set; }

        Task AddJourney(WasteJourney journey);

        Task UpdateJourneySiteId(int journeyId, int siteId);

        Task UpdateJourneyBaledWithWire(int journeyId, bool baledWithWire);

        Task UpdateJourneyTonnage(int journeyId, double journeyTonnage);

        Task UpdateJourneyDoneId(int journeyId, DoneWaste selectedDoneWaste);

        Task UpdateJourneyWasteTypeId(int journeyId, int wasteTypeId);

        Task UpdateJourneyMonth(int journeyId, int selectedMonth);

        Task<IList<WasteType>> GetAllWasteTypes();

        Task<string> GetWasteTypeName(int journeyId);

        Task<int?> GetWasteTypeId(int journeyId);

        Task<DoneWaste?> GetDoneWaste(int journeyId);

        Task<WasteJourney> GetWasteJourneyById(int jounreyId);

        Task<double?> GetWasteTonnage(int journeyId);

        Task<bool> Exists(int journeyId);
    }
}