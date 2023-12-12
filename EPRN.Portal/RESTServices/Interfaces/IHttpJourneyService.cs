using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpJourneyService
    {
        Task<int> CreateJourney();

        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId);

        Task<DoneWaste> GetWhatHaveYouDoneWaste(int journeyId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task SaveTonnage(int journeyId, double tonnage);
        Task SaveBaledWithWire(int journeyId, bool baledWithWire);

        Task SaveReprocessorExport(int journeyId, int siteId);

        Task<int?> GetWasteTypeId(int journeyId);

        Task<double?> GetWasteTonnage(int journeyId);
    }
}
