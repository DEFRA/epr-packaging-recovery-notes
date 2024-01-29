using EPRN.Common.Dtos;
using EPRN.Common.Enums;
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

        public async Task<int> CreatePrnRecord(
            int materialId,
            Category category)
        {
            return await Post<int>($"Create/Category/{(int)category}/Material/{materialId}");
        }

        public async Task<double?> GetPrnTonnage(
            int id)
        {
            return await Get<double?>($"{id}/Tonnage");
        }

        public async Task SaveTonnage(
            int id,
            double tonnage)
        {
            await Post($"{id}/Tonnage/{tonnage}");
        }

        public async Task<ConfirmationDto> GetConfirmation(
            int id)
        {
            return await Get<ConfirmationDto>($"{id}/Confirmation");
        }

        public async Task<CheckYourAnswersDto> GetCheckYourAnswers(
            int id)
        {
            return await Get<CheckYourAnswersDto>($"{id}/check");
        }

        public async Task SaveCheckYourAnswers(int id)
        {
            await Post($"{id}/check");
        }

        public async Task<List<SentPrnsDto>> GetSentPrns(
            int? page,
            int pageSize,
            string? searchTerm,
            string? filterBy,
            string? sortBy)
        {
            return await Get<List<SentPrnsDto>>($"GetSentPrns/Page/{page}/PageSize/{pageSize}/SearchTerm/{searchTerm}/FilterBy/{filterBy}/SortBy/{sortBy}");
        }
    }
}