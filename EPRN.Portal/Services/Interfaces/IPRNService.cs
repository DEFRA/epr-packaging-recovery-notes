using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IPRNService
    {
        TonnesViewModel GetTonnesViewModel(int id);

        Task SaveTonnes(TonnesViewModel tonnesViewModel);

        Task<CreateViewModel> GetCreateViewModel();
    }
}
