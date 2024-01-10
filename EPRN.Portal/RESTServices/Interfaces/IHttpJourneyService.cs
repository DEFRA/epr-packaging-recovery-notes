using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.ViewModels;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpJourneyService
    {
        Task<int> CreateJourney();

        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId);

        Task SaveSelectedWasteSubType(int journeyId, int selectedWasteSubTypeId, double adjustment);

        Task<DoneWaste> GetWhatHaveYouDoneWaste(int journeyId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task<int?> GetWasteTypeId(int journeyId);

        Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int journeyId);

        Task SaveTonnage(int journeyId, double tonnage);

        Task<GetBaledWithWireDto> GetBaledWithWire(int journeyId);

        Task SaveBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage);

        Task SaveReprocessorExport(int journeyId, int siteId);

        Task<double?> GetWasteTonnage(int journeyId);

        Task SaveNote(int journeyId, string noteContent);

        Task<string> GetNote(int journeyId);
    }
}
