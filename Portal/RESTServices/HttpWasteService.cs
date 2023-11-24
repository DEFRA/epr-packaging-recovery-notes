using EPRN.Common.Dtos;
using Portal.RESTServices.Interfaces;
using Portal.Services;
using Portal.ViewModels;

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

        public async Task<WasteRecordStatusViewModel?> GetWasteRecordStatus(string reprocessorId)
        {
            var uri = $"";
            return await Get<WasteRecordStatusDto>(uri);
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            await Post($"Journey/{journeyId}/Month/{selectedMonth}", null);
        }
    }
}
