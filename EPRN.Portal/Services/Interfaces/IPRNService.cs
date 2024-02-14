using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IPRNService
    {
        Task<int> CreatePrnRecord(int materialId, Category category);

        Task<TonnesViewModel> GetTonnesViewModel(int id);

        Task SaveTonnes(TonnesViewModel tonnesViewModel);

        Task<CreatePrnViewModel> CreatePrnViewModel();

        Task<ConfirmationViewModel> GetConfirmation(int id);

        Task<CheckYourAnswersViewModel> GetCheckYourAnswersViewModel(int id);

        Task SaveCheckYourAnswers(int id);

        Task<PrnSavedAsDraftViewModel> GetDraftPrnConfirmationModel(int id);

        Task<CancelViewModel> GetCancelViewModel(int id);

        Task<RequestCancelViewModel> GetRequestCancelViewModel(int id);

        Task RequestToCancelPRN(RequestCancelViewModel requestCancelViewModel);

        Task CancelPRN(CancelViewModel cancelViewModel);

        Task<ViewSentPrnsViewModel> GetViewSentPrnsViewModel(GetSentPrnsViewModel request);

        Task<ViewPRNViewModel> GetViewPrnViewModel(string reference);

        Task<(DecemberWasteViewModel, bool)> GetDecemberWasteModel(int id);

        Task SaveDecemberWaste(DecemberWasteViewModel decemberWasteModel);

        Task<ActionPrnViewModel> GetActionPrnViewModel(int id);

        Task<DeleteDraftPrnViewModel> GetDeleteDraftPrnViewModel(int id);
    }
}
