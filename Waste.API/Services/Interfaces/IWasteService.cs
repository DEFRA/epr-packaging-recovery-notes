using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace Waste.API.Services.Interfaces
{
    public interface IWasteService
    {
        Task<IEnumerable<WasteTypeDto>> WasteTypes();

        Task<int> CreateJourney();

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveWasteType(int journeyId, int wasteTypeId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);

        Task SaveTonnage(int journeyId, double tonnage);
        Task<WasteRecordStatusDto?> GetWasteRecordStatus(int journeyId);

        Task SaveBaledWithWire(int journeyId, bool baledWithWire);
    }
}
