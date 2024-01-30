using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpJourneyService
    {
        Task<int> CreateJourney(
            int materialId,
            Category category);

        Task<JourneyAnswersDto> GetJourneyAnswers(int journeyId);

        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);

        Task<int?> GetSelectedMonth(int journeyId);

        Task<QuarterlyDatesDto> GetQuarterlyMonths(int journeyId, int currentMonth, bool hasSubmittedPreviousQuarterReturn);

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId);

        Task SaveSelectedWasteSubType(int journeyId, int selectedWasteSubTypeId, double adjustment);

        Task<DoneWaste> GetWhatHaveYouDoneWaste(int journeyId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task<int?> GetWasteTypeId(int journeyId);

        Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int journeyId);

        Task SaveTonnage(int journeyId, double tonnage);

        Task<BaledWithWireDto> GetBaledWithWire(int journeyId);

        Task SaveBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage);

        Task SaveReprocessorExport(int journeyId, int siteId);

        Task<double?> GetWasteTonnage(int journeyId);

        Task SaveNote(int journeyId, string noteContent);

        Task<NoteDto> GetNote(int journeyId);

        Task<Category> GetCategory(int journeyId);

        Task SaveDecemberWaste(int journeyId, bool decemberWaste);
    }
}
