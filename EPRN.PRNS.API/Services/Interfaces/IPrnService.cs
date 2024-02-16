using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.PRNS.API.Services.Interfaces
{
    public interface IPrnService
    {
        Task<bool> PrnRecordExists(
            int id,
            Category category);

        Task<int> CreatePrnRecord(
            int materialId,
            Category category);

        Task<double?> GetTonnage(int id);

        Task SaveTonnage(int id, double tonnage);

        Task<ConfirmationDto> GetConfirmation(int id);

        Task<CheckYourAnswersDto> GetCheckYourAnswers(int id);

        Task<PrnStatus> GetStatus(int id);

        Task<StatusAndProducerDto> GetStatusWithProducerName(int id);

        Task SaveCheckYourAnswers(int id);

        Task CancelPrn(int id, string reason);

        Task RequestCancelPrn(int id, string reason);

        Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request);

        Task<PRNDetailsDto> GetPrnDetails(string reference);

        Task<DecemberWasteDto> GetDecemberWaste(int journeyId);

        Task SaveDecemberWaste(int jouneyId, bool decemberWaste);

        Task<DeleteDraftPrnDto> GetPrnReference(int id);

        Task DeleteDraftPrn(int id);

        Task<DraftDetailsPrnDto> GetDraftDetails(int id);

        Task SaveDraftPrn(int id);
    }
}
