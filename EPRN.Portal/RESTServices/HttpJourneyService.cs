﻿#nullable enable
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;

namespace EPRN.Portal.RESTServices
{
    public class HttpJourneyService : BaseHttpService, IHttpJourneyService
    {
        public HttpJourneyService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            string baseUrl,
            string endPointName) : base(httpContextAccessor, httpClientFactory, baseUrl, endPointName)
        {
        }

        public async Task<int> CreateJourney(
            int materialId,
            Category category,
            string companyReferenceId)
        {
            return await Post<int>($"Create/Material/{materialId}/Category/{category}/{companyReferenceId}");
        }

        public async Task<JourneyAnswersDto> GetJourneyAnswers(int journeyId)
        {
            return await Get<JourneyAnswersDto>($"{journeyId}/JourneyAnswers");
        }

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId)
        {
            return await Get<WasteRecordStatusDto>($"{journeyId}/status");
        }
        
        public async Task<QuarterlyDatesDto> GetQuarterlyMonths(int journeyId, int currentMonth, bool hasSubmittedPreviousQuarterReturn)
        {
            return await Get<QuarterlyDatesDto>($"{journeyId}/QuarterlyDates/{currentMonth}/{hasSubmittedPreviousQuarterReturn}");
        }

        public async Task<int?> GetSelectedMonth(int journeyId)
        {
            return await Get<int?>($"{journeyId}/Month");
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            await Post($"{journeyId}/Month/{selectedMonth}");
        }

        public async Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId)
        {
            await Post($"{journeyId}/Type/{selectedWasteTypeId}");
        }

        public async Task SaveSelectedWasteSubType(int journeyId, int selectedWasteSubTypeId, double adjustment)
        {
            await Post($"{journeyId}/SubTypes/{selectedWasteSubTypeId}/Adjustment/{adjustment}");
        }

        public async Task<DoneWaste> GetWhatHaveYouDoneWaste(int journeyId)
        {
            return await Get<DoneWaste>($"{journeyId}/WhatHaveYouDoneWaste");
        }

        public async Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste)
        {
            await Post($"{journeyId}/Done/{whatHaveYouDoneWaste}");
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            return await Get<string>($"{journeyId}/WasteType");
        }

        public async Task<int?> GetWasteTypeId(int journeyId)
        {
            return await Get<int?>($"{journeyId}/Type");
        }

        public async Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int journeyId)
        {
            return await Get<WasteSubTypeSelectionDto>($"{journeyId}/Subtype");
        }

        public async Task<WasteTonnageDto> GetWasteTonnage(int journeyId)
        {
            return await Get<WasteTonnageDto>($"{journeyId}/Tonnage");
        }

        public async Task SaveTonnage(int journeyId, double tonnage)
        {
            await Post($"{journeyId}/Tonnage/{tonnage}");
        }

        public async Task<BaledWithWireDto> GetBaledWithWire(int journeyId)
        {
            return await Get<BaledWithWireDto>($"{journeyId}/BaledWithWire");
        }

        public async Task SaveBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage)
        {
            await Post($"{journeyId}/BaledWithWire/{baledWithWire}/{baledWithWireDeductionPercentage}");
        }

        public async Task SaveReprocessorExport(int journeyId, int siteId)
        {
            await Post($"{journeyId}/ReProcessorExport/{siteId}");
        }

        public async Task SaveNote(int journeyId, string noteContent)
        {
            await Post($"{journeyId}/Note/{noteContent}");
        }

        public async Task<NoteDto> GetNote(int journeyId)
        {
            return await Get<NoteDto>($"{journeyId}/Note");
        }

        public async Task<Category> GetCategory(int journeyId)
        {
            return (Category)await Get<Category?>($"{journeyId}/Category");
        }

        public async Task<AccredidationLimitDto> GetAccredidationLimit(int journeyId, string userReferenceId, double newQuantityEntered)
        {
            return await Get<AccredidationLimitDto>($"{journeyId}/UserAccredidationLimit/{userReferenceId}/{newQuantityEntered}");
        }

    }
}
