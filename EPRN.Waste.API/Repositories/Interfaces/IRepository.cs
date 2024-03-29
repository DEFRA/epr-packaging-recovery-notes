﻿using EPRN.Common.Data.DataModels;
using EPRN.Common.Data.Enums;
using EPRN.Common.Dtos;


namespace EPRN.Waste.API.Repositories.Interfaces
{
    public interface IRepository
    {

        Task AddJourney(WasteJourney journey);

        Task UpdateJourneySiteId(int journeyId, int siteId);

        Task UpdateJourneyBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage);

        Task UpdateJourneyTonnage(int journeyId, double journeyTonnage);

        Task UpdateJourneyDoneId(int journeyId, DoneWaste selectedDoneWaste);

        Task UpdateJourneyWasteTypeId(int journeyId, int wasteTypeId);

        Task UpdateJourneyMonth(int journeyId, int selectedMonth);

        Task UpdateJourneySubTypeAndAdjustment(
            int journeyId,
            int subTypeId,
            double adjustment);

        Task UpdateJourneyCategory(int journeyId, Category category);

        Task<IList<WasteTypeDto>> GetAllWasteTypes();

        Task<IList<WasteSubTypeDto>> GetWasteSubTypes(int wasteTypeId);

        Task<string> GetWasteTypeName(int journeyId);

        Task<int?> GetWasteTypeId(int journeyId);

        Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int journeyId);

        Task<int?> GetSelectedMonth(int journeyId);

        Task<DoneWaste?> GetDoneWaste(int journeyId);

        Task<WasteRecordStatusDto> GetWasteRecordStatus(int jounreyId);

        Task<BaledWithWireDto> GetBaledWithWire(int journeyId);

        Task<WasteJourney> GetWasteJourneyById_FullModel(int journeyId);

        Task<JourneyAnswersDto> GetWasteJourneyAnswersById(int journeyId);

        Task<WasteTonnageDto> GetWasteTonnage(int journeyId);

        Task<bool> Exists(int journeyId);

        Task<NoteDto> GetWasteNote(int journeyId);

        Task UpdateWasteNote(int journeyId, string note);

        Task<Category> GetCategory(int journeyId);
        
        Task<double?> GetTotalQuantityForAllUserJourneys(string userReferenceId);
    }
}