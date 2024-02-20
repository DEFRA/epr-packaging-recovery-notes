using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpPrnsService
    {
        Task<double?> GetPrnTonnage(
            int id);

        Task SaveTonnage(
            int id,
            double tonnage);

        Task<ConfirmationDto> GetConfirmation(
            int id);

        Task<CheckYourAnswersDto> GetCheckYourAnswers(
            int id);

        Task<int> CreatePrnRecord(
            int materialId,
            Category category, 
            string userReferenceId);

        Task SaveCheckYourAnswers(
            int id);

        Task<PrnStatus> GetStatus(int id);

        Task CancelPRN(
            int id,
            string reason);

        Task<StatusAndProducerDto> GetStatusAndProducer(int id);

        Task RequestCancelPRN(
            int id,
            string reason);

        Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request);

        Task<PRNDetailsDto> GetPrnDetails(string reference);

        Task<DecemberWasteDto> GetDecemberWaste(int id);

        Task SaveDecemberWaste(int journeyId, bool decemberWaste);

        Task<DeleteDraftPrnDto> GetPrnReference(int id);

        Task DeleteDraftPrn(int id);

        Task<DraftDetailsPrnDto> GetDraftDetails(int id);

        Task SaveDraftPrn(int id);
        Task<List<PrnDto>> GetDraftPrnDetailsForUser(string userReferenceId);
    }
}
