using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord(
            int materialType,
            Category category,
            string prnReference);

        Task<bool> PrnExists(
            int id,
            Category category);

        Task<double?> GetTonnage(int id);

        Task UpdateTonnage(int id, double tonnes);

        Task<ConfirmationDto> GetConfirmation(int id);

        Task<CheckYourAnswersDto> GetCheckYourAnswersData(int id);

        Task<PrnStatus> GetStatus(int id);

        Task<StatusAndProducerDto> GetStatusAndRecipient(int id);

        Task UpdatePrnStatus(
            int id,
            PrnStatus status,
            string reason = null);

        Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request);

        Task<PRNDetailsDto> GetDetails(string reference);


        Task<DecemberWasteDto> GetDecemberWaste(int id);

        Task SaveDecemberWaste(int jouneyId, bool decemberWaste);

        Task<DeleteDraftPrnDto> GetPrnReference(int id);

        Task DeleteDraftPrn(int id);
    }
}