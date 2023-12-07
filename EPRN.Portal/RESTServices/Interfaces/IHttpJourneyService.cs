using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpJourneyService
    {
        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);

        Task SaveSelectedMonth(int journeyId, int selectedMonth, DoneWaste whatHaveYouDoneWaste);

        Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task SaveTonnage(int journeyId, double tonnage);
    }
}
