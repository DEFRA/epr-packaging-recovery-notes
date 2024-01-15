using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;

namespace EPRN.Portal.RESTServices
{
    public class HttpPrnsService : BaseHttpService, IHttpPrnsService
    {
        private Category _category;

        public HttpPrnsService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            Category category,
            string baseUrl,
            string endPointName) : base(httpContextAccessor, httpClientFactory, baseUrl, endPointName)
        {
            _category = category;
        }

        public async Task<int> CreatePrnRecord(
            int materialId)
        {
            return await Post<int>($"Create/Category/{(int)_category}/Material/{materialId}");
        }

        public async Task<double?> GetPrnTonnage(
            int id)
        {
            return await Get<double?>($"{id}/Category/{(int)_category}/Tonnage");
        }

        public async Task SaveTonnage(
            int id,
            double tonnage)
        {
            await Post($"{id}/Category/{(int)_category}/Tonnage/{tonnage}");
        }

        public async Task<ConfirmationDto> GetConfirmation(
            int id)
        {
            return await Get<ConfirmationDto>($"{id}/Category/{(int)_category}/Confirmation");
        }
    }
}