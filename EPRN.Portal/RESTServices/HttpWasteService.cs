using EPRN.Common.Dtos;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;

namespace EPRN.Portal.RESTServices
{
    public class HttpWasteService : BaseHttpService, IHttpWasteService
    {
        public HttpWasteService(
            IHttpContextAccessor httpContextAccessor,
            string baseUrl,
            IHttpClientFactory httpClientFactory) : base(httpContextAccessor, baseUrl, httpClientFactory)
        {
        }

        public async Task<IEnumerable<WasteTypeDto>> GetWasteMaterialTypes()
        {
            return await Get<List<WasteTypeDto>>("Types");
        }

        public async Task<IEnumerable<WasteSubTypeDto>> GetWasteMaterialSubTypes(int wasteTypeId)
        {
            return await Get<List<WasteSubTypeDto>>($"Types/{wasteTypeId}/SubTypes");
        }
    }
}
