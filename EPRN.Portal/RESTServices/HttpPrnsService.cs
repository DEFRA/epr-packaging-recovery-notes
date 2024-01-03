using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;

namespace EPRN.Portal.RESTServices
{
    public class HttpPrnsService : BaseHttpService, IHttpPrnsService
    {
        public HttpPrnsService(
            IHttpContextAccessor httpContextAccessor,
            string baseUrl,
            IHttpClientFactory httpClientFactory) : base(httpContextAccessor, baseUrl, httpClientFactory)
        {
        }

        public async Task SaveTonnage(int id, double tonnage)
        {
            await Post($"{id}/Tonnage/{tonnage}");
        }
    }
}
