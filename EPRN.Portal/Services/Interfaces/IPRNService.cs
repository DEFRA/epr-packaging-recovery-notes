using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IPRNService
    {
        public TonnesViewModel GetTonnesViewModel(int id);
    }
}
