using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;

namespace EPRN.Portal.RESTServices
{
    public class HttpPrnsService : BaseHttpService, IHttpPrnsService
    {
        public HttpPrnsService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            string baseUrl,
            string endPointName) : base(httpContextAccessor, httpClientFactory, baseUrl, endPointName)
        {
        }

        public async Task<int> CreatePrnRecord(int materialId)
        {
            return await Post<int>($"Create/{materialId}/Material");
        }

        public async Task<double?> GetPrnTonnage(int id)
        {
            return await Get<double?>($"{id}/Tonnage");
        }

        public async Task SaveTonnage(int id, double tonnage)
        {
            await Post($"{id}/Tonnage/{tonnage}");
        }
    }
}