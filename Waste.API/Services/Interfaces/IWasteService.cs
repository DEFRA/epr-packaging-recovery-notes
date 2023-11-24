namespace Waste.API.Services.Interfaces
{
    public interface IWasteService
    {
        Task<IEnumerable<WasteTypeDto>> WasteTypes();

        Task<int> CreateJourney();

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveWasteType(int journeyId, int wasteTypeId);

        Task SaveSelectedWasteType(int journeyId, String selectedWasteType);

        Task<string> GetWasteType(int journeyId);
    }
}
