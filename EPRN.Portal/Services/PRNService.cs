using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services
{
    public class PRNService : IPRNService
    {
        public TonnesViewModel GetTonnesViewModel(int id)
        {
            return new TonnesViewModel
            {
                JourneyId = id,
            };
        }
    }
}
