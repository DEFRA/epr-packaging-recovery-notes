using EPRN.Common.Dtos;
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
    }
}
