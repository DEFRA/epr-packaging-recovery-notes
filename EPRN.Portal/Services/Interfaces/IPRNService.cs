using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IPRNService
    {
        Task<TonnesViewModel> GetTonnesViewModel(int id);

        Task SaveTonnes(TonnesViewModel tonnesViewModel);
    }
}
