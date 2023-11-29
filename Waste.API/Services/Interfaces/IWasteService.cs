using EPRN.Common.Dtos;

namespace Waste.API.Services.Interfaces
{
    public interface IWasteService
    {
        Task<IEnumerable<WasteTypeDto>> WasteTypes();

        Task<int> CreateJourney();

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveWasteType(int journeyId, int wasteTypeId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, String whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task<WasteRecordStatusDto?> GetWasteRecordStatus(int journeyId);
    }
}
