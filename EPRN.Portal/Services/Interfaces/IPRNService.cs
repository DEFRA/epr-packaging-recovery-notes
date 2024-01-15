using EPRN.Common.Enums;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IPRNService
    {
        Task<TonnesViewModel> GetTonnesViewModel(int id);

        Task SaveTonnes(TonnesViewModel tonnesViewModel);

        Task<CreatePrnViewModel> CreatePrnViewModel();

        Task<ConfirmationViewModel> GetConfirmation(int id);

        Task<int> CreatePrnRecord(int materialId);
    }
}
