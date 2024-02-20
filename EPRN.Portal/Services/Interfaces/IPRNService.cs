using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IPRNService
    {
        Task<int> CreatePrnRecord(int materialId, Category category, string userReferenceId);

        Task<TonnesViewModel> GetTonnesViewModel(int id);

        Task SaveTonnes(TonnesViewModel tonnesViewModel);

        Task<CreatePrnViewModel> CreatePrnViewModel();

        Task<ConfirmationViewModel> GetConfirmation(int id);

        Task<CheckYourAnswersViewModel> GetCheckYourAnswersViewModel(int id);

        Task SaveCheckYourAnswers(int id);

        Task<CancelViewModel> GetCancelViewModel(int id);

        Task<RequestCancelViewModel> GetRequestCancelViewModel(int id);

        Task RequestToCancelPRN(RequestCancelViewModel requestCancelViewModel);

        Task CancelPRN(CancelViewModel cancelViewModel);

        Task<ViewSentPrnsViewModel> GetViewSentPrnsViewModel(GetSentPrnsViewModel request);

        Task<ViewPRNViewModel> GetViewPrnViewModel(string reference);

        Task<(DecemberWasteViewModel, bool)> GetDecemberWasteModel(int id);

        Task SaveDecemberWaste(DecemberWasteViewModel decemberWasteModel);

        Task<DeleteDraftPrnViewModel> GetDeleteDraftPrnViewModel(int id);

        Task DeleteDraftPrn(DeleteDraftPrnViewModel viewModel);
        Task<DraftConfirmationViewModel> GetDraftConfirmationViewModel(int id);

        Task SaveDraftPrn(DraftConfirmationViewModel draftConfirmationViewModel);

        Task<List<ViewDraftPrnViewModel>> GetDraftViewPrnViewModel(string userReferenceId);
    }
}
