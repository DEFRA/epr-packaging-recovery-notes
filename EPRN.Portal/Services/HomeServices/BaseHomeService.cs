using EPRN.Portal.Configuration;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using Microsoft.Extensions.Options;

namespace EPRN.Portal.Services
{
    public abstract class BaseHomeService : IHomeService
    {
        protected IOptions<AppConfigSettings> ConfigSettings;

        public BaseHomeService(IOptions<AppConfigSettings> configSettings)
        {
            if (configSettings == null)
                return;

            if (configSettings.Value == null)
                throw new ArgumentNullException(nameof(configSettings));

            if (configSettings.Value.DeductionAmount_Exporter == null)
                throw new ArgumentNullException(nameof(configSettings.Value.DeductionAmount_Exporter));

            if (configSettings.Value.DeductionAmount_Reprocessor == null)
                throw new ArgumentNullException(nameof(configSettings.Value.DeductionAmount_Reprocessor));

            if (configSettings.Value.DeductionAmount_ExporterAndReprocessor == null)
                throw new ArgumentNullException(nameof(configSettings.Value.DeductionAmount_ExporterAndReprocessor));

            ConfigSettings = configSettings;
        }

        public virtual async Task<HomePageViewModel> GetHomePage()
        {
            // TODO: Replace with actual data in the future
            var homePageViewModel = new HomePageViewModel
            {
                Name = "Green LTD",
                ContactName = "John Watson",
                AccountNumber = "12 Head office St, Liverpool, L12 345 - 0098678"
            };

            homePageViewModel.CardViewModels = GetCardViewModels();

            return homePageViewModel;
        }

        protected abstract List<CardViewModel> GetCardViewModels();

        protected CardViewModel GetCardViewModel(string title, string description)
        {
            var cardViewModel = new CardViewModel
            {
                Title = title,
                Description = description
            };

            return cardViewModel;
        }

        public abstract double? GetBaledWithWireDeductionPercentage();

    }
}
