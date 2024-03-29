﻿
using EPRN.Common.Dtos;
using EPRN.Common.Enums;


namespace EPRN.Waste.API.Services.Interfaces
{
    public interface IJourneyService
    {
        Task<int> CreateJourney(int materialId, Category category, string companyReferenceId);

        Task<int?> GetSelectedMonth(int journeyId);

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveWasteType(int journeyId, int wasteTypeId);

        Task SaveWasteSubType(int journeyId, int wasteSubTypeId, double adjustment);

        Task<DoneWaste?> GetWhatHaveYouDoneWaste(int journeyId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task<int?> GetWasteTypeId(int journeyId);

        Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int journeyId);

        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);

        Task SaveTonnage(int journeyId, double tonnage);

        Task SaveBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage);

        Task SaveReprocessorExport(int journeyId, int siteId);

        Task<WasteTonnageDto> GetTonnage(int journeyId);

        Task<bool> JourneyExists(int journeyId);

        Task<NoteDto> GetWasteRecordNote(int journeyId);
        Task SaveWasteRecordNote(int journeyId, string note);

        Task<object?> GetCategory(int journeyId);

        Task<BaledWithWireDto> GetBaledWithWire(int journeyId);

        Task<JourneyAnswersDto> GetJourneyAnswers(int journeyId);

        Task<AccredidationLimitDto> GetAccredidationLimit(string userReferenceId, double newQuantityEntered);
        
    }
}
