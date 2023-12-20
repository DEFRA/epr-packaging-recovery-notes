using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services
{
    public abstract class PRNService : IPRNService
    {
        public TonnesViewModel GetTonnesViewModel(int id)
        {
            return new TonnesViewModel
            {
                JourneyId = id,
            };
        }

        public Task SaveTonnes(TonnesViewModel tonnesViewModel)
        {
            throw new NotImplementedException();
        }
    }
}