using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using System.Security.Cryptography.Xml;

namespace EPRN.Portal.RESTServices
{
    public class HttpPrnsService : BaseHttpService, IHttpPrnsService
    {
        private Category _category;

        public HttpPrnsService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            string baseUrl,
            string endPointName) : base(httpContextAccessor, httpClientFactory, baseUrl, endPointName)
        {
        }

        /// <summary>
        /// Overloaded constructor where category is needed for validating against a PRN id
        /// </summary>
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
            int materialId,
            Category category, 
            string userReferenceId)
        {
            return await Post<int>($"Create/Category/{(int)category}/Material/{materialId}/{userReferenceId}");
        }

        public async Task<double?> GetPrnTonnage(
            int id)
        {
            return await Get<double?>($"{id}/Category/{_category}/Tonnage");
        }

        public async Task SaveTonnage(
            int id,
            double tonnage)
        {
            await Post($"{id}/Category/{_category}/Tonnage/{tonnage}");
        }

        public async Task<ConfirmationDto> GetConfirmation(
            int id)
        {
            return await Get<ConfirmationDto>($"{id}/Category/{_category}/Confirmation");
        }

        public async Task<CheckYourAnswersDto> GetCheckYourAnswers(
            int id)
        {
            return await Get<CheckYourAnswersDto>($"{id}/Category/{_category}/check");
        }

        public async Task SaveCheckYourAnswers(int id)
        {
            await Post($"{id}/Category/{_category}/check");
        }

        public async Task<PrnStatus> GetStatus(int id)
        {
            return await Get<PrnStatus>($"{id}/Category/{_category}/Status");
        }

        public async Task CancelPRN(int id, string reason)
        {
            await Post($"{id}/Category/{_category}/Cancel", reason);
        }

        public async Task<StatusAndProducerDto> GetStatusAndProducer(int id)
        {
            return await Get<StatusAndProducerDto>($"{id}/Category/{_category}/StatusAndProducer");
        }

        public async Task RequestCancelPRN(int id, string reason)
        {
            await Post($"{id}/Category/{_category}/RequestCancel", reason);
        }

        public async Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request)
        {
            return await Get<SentPrnsDto>($"GetSentPrns{BuildUrlWithQueryString(request)}", false);
        }

        public async Task<PRNDetailsDto> GetPrnDetails(string reference)
        {
            return await Get<PRNDetailsDto>($"Category/{_category}/Details/{reference}");
        }

        public async Task<DecemberWasteDto> GetDecemberWaste(int id)
        {
            return await Get<DecemberWasteDto>($"{id}/Category/{_category}/DecemberWaste");
        }

        public async Task SaveDecemberWaste(int id, bool decemberWaste)
        {
            await Post($"{id}/Category/{_category}/DecemberWaste/{decemberWaste}");
        }

        public async Task<DeleteDraftPrnDto> GetPrnReference(int id)
        {
            return await Get<DeleteDraftPrnDto>($"{id}/Category/{_category}/PrnReference");
        }

        public async Task DeleteDraftPrn(int id)
        {
            await Post($"{id}/Category/{_category}/DeleteDraftPrn");
        }

        public async Task<DraftDetailsPrnDto> GetDraftDetails(int id)
        {
            return await Get<DraftDetailsPrnDto>($"{id}/Category/{_category}/Draft");
        }

       public async Task SaveDraftPrn(int id)
        {
            await Post($"{id}/Category/{_category}/Draft");
        }

        public async Task<List<PrnDto>> GetDraftPrnDetailsForUser(string userReferenceId)
        {
            return await Get<List<PrnDto>>($"Category/{_category}/DraftDetails?userReferenceId={userReferenceId}", false);
        }
    }
}