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

        public async Task<int?> GetPrnTonnage(int id)
        {
            return await Get<int?>($"{id}/Tonnage");
        }

        public async Task SaveTonnage(int id, int tonnage)
        {
            await Post($"{id}/Tonnage/{tonnage}");
        }
    }
}