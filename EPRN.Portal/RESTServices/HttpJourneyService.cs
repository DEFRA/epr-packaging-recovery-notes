using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;

namespace EPRN.Portal.RESTServices
{
    public class HttpJourneyService : BaseHttpService, IHttpJourneyService
    {
        public HttpJourneyService(
            string baseUrl, 
            IHttpClientFactory httpClientFactory) : base(baseUrl, httpClientFactory)
        {
        }

        public async Task<int> CreateJourney()
        {
            return await Post<int>("Create");
        }

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId)
        {
            return await Get<WasteRecordStatusDto>($"{journeyId}/status");
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            await Post($"{journeyId}/Month/{selectedMonth}");
        }

        public async Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId)
        {
            await Post($"{journeyId}/Type/{selectedWasteTypeId}");
        }

        public async Task<DoneWaste> GetWhatHaveYouDoneWaste(int journeyId)
        {
            return await Get<DoneWaste>($"{journeyRoutePart}/{journeyId}/WhatHaveYouDoneWaste");
        }

        public async Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste)
        {
            await Post($"{journeyId}/Done/{whatHaveYouDoneWaste}");
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            return await Get<string>($"{journeyId}/WasteType");
        }

        public async Task SaveTonnage(int journeyId, double tonnage)
        {
            await Post($"{journeyId}/Tonnage/{tonnage}");
        }

        public async Task SaveBaledWithWire(int journeyId, bool bailedWithWire)
        {
            await Post($"Journey/{journeyId}/BaledWithWire/{bailedWithWire}");
        }
    }
}
