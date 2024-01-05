using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services
{
    public class PRNService : IPRNService
    {
        private IHttpPrnsService _httpPrnsService;

        public PRNService(IHttpPrnsService httpPrnsService)
        {
            _httpPrnsService = httpPrnsService ?? throw new ArgumentNullException(nameof(httpPrnsService));
        }

        public async Task<TonnesViewModel> GetTonnesViewModel(int id)
        {
            return new TonnesViewModel
            {
                JourneyId = id,
                Tonnes = await _httpPrnsService.GetPrnTonnage(id)
            };
        }

        public async Task SaveTonnes(TonnesViewModel tonnesViewModel)
        {
            await _httpPrnsService.SaveTonnage(
                tonnesViewModel.JourneyId,
                tonnesViewModel.Tonnes.Value);
        }
    }
}