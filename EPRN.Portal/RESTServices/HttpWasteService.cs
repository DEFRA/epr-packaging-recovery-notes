using EPRN.Common.Dtos;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;

namespace EPRN.Portal.RESTServices
{
    public class HttpWasteService : BaseHttpService, IHttpWasteService
    {
        public HttpWasteService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            string baseUrl,
            string endPointName) : base(httpContextAccessor, httpClientFactory, baseUrl, endPointName)
        {
        }

        public async Task<Dictionary<int, string>> GetWasteMaterialTypes()
        {
            return await Get<Dictionary<int, string>>("Types");
        }

        public async Task<IEnumerable<WasteSubTypeDto>> GetWasteMaterialSubTypes(int wasteTypeId)
        {
            return await Get<List<WasteSubTypeDto>>($"Types/{wasteTypeId}/SubTypes");
        }
    }
}
