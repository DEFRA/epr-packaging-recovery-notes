using EPRN.Portal.Configuration;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

namespace EPRN.Portal.Services
{
    public abstract class UserBasedBaseService : IUserBasedService
    {
        protected IOptions<AppConfigSettings> ConfigSettings;
        protected IHttpJourneyService _httpJourneyService;
        protected IUrlHelper UrlHelper;

        public UserBasedBaseService(IOptions<AppConfigSettings> configSettings, 
            IHttpJourneyService httpJourneyService,
            IUrlHelperFactory urlHelperFactory, 
            IActionContextAccessor actionContextAccessor)
        {
            if (configSettings.Value == null)
                throw new ArgumentNullException(nameof(configSettings));

            if (configSettings.Value.DeductionAmount_Exporter == null)
                throw new ArgumentNullException(nameof(configSettings.Value.DeductionAmount_Exporter));

            if (configSettings.Value.DeductionAmount_Reprocessor == null)
                throw new ArgumentNullException(nameof(configSettings.Value.DeductionAmount_Reprocessor));

            if (configSettings.Value.DeductionAmount_ExporterAndReprocessor == null)
                throw new ArgumentNullException(nameof(configSettings.Value.DeductionAmount_ExporterAndReprocessor));

            ConfigSettings = configSettings;

            UrlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);

            _httpJourneyService = httpJourneyService ?? throw new ArgumentNullException(nameof(_httpJourneyService));

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

        public abstract Task<CYAViewModel> GetCheckAnswers(int journeyId);
    }
}
