﻿using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Waste.API.Services.Interfaces
{
    public interface IJourneyService
    {
        Task<int> CreateJourney();

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveWasteType(int journeyId, int wasteTypeId);

        Task<DoneWaste> GetWhatHaveYouDoneWaste(int journeyId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);

        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);

        Task SaveTonnage(int journeyId, double tonnage);
        
        Task SaveBaledWithWire(int journeyId, bool baledWithWire);

        Task SaveReprocessorExport(int journeyId, int siteId);
    }
}