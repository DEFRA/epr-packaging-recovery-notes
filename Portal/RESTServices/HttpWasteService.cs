using EPRN.Common.Dtos;
using EPRN.Common.Enum;
using Portal.RESTServices.Interfaces;
using Portal.Services;

namespace Portal.RESTServices
{
    public class HttpWasteService : BaseHttpService, IHttpWasteService
    {
        public HttpWasteService(
            string baseUrl,
            IHttpClientFactory httpClientFactory) : base(baseUrl, httpClientFactory)
        {
        }

        public async Task<IEnumerable<WasteTypeDto>> GetWasteMaterialTypes()
        {
            return await Get<List<WasteTypeDto>>("WasteTypes");
        }

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId)
        {
            return await Get<WasteRecordStatusDto>($"Journey/{journeyId}/status");
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            await Post($"Journey/{journeyId}/Month/{selectedMonth}");
        }

        public async Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId)
        {
            await Post($"Journey/{journeyId}/Type/{selectedWasteTypeId}");
        }

        public async Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste)
        {
            await Post($"Journey/{journeyId}/Done/{whatHaveYouDoneWaste}");
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            return await Get<string>($"Journey/{journeyId}/WasteType");
        }
    }
}
