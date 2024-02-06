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

        public async Task<PrnStatus> GetStatus(int id)
        {
            return await Get<PrnStatus>($"{id}/Status");
        }

        public async Task CancelPRN(int id, string reason)
        {
            await Post($"{id}/Cancel", reason);
        }

        public async Task<StatusAndProducerDto> GetStatusAndProducer(int id)
        {
            return await Get<StatusAndProducerDto>($"{id}/StatusAndProducer");
        }

        public async Task RequestCancelPRN(int id, string reason)
        {
            await Post($"{id}/RequestCancel", reason);
        }

        public async Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request)
        {
            return await Get<SentPrnsDto>($"GetSentPrns{BuildUrlWithQueryString(request)}", false);
        }

        public async Task<PRNDetailsDto> GetPrnDetails(string reference)
        {
            return await Get<PRNDetailsDto>($"Details/{reference}");
        }

        public async Task<DecemberWasteDto> GetDecemberWaste(int id)
        {
            return await Get<DecemberWasteDto>($"{id}/DecemberWaste");
        }
        
        public async Task SaveDecemberWaste(int id, bool decemberWaste)
        {
            await Post($"{id}/DecemberWaste/{decemberWaste}");
        }
    }
}