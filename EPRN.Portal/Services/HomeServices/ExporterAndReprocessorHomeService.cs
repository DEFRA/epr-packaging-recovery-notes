using EPRN.Portal.Configuration;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using static EPRN.Common.Constants.Strings;

namespace EPRN.Portal.Services
{
    public class ExporterAndReprocessorHomeService : UserBasedBaseService, IUserBasedService
    {
        public ExporterAndReprocessorHomeService(IOptions<AppConfigSettings> configSettings,
            IHttpJourneyService httpJourneyService,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
            : base(configSettings, httpJourneyService, urlHelperFactory, actionContextAccessor)
        {
        }

        protected override List<CardViewModel> GetCardViewModels()
        {
            var wasteCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Waste_Link_RecordWaste, UrlHelper.Action(
                    Routes.Actions.Waste.RecordWaste,
                    Routes.Controllers.Waste) },
                { HomePageResources.HomePage_Waste_Link_ViewEditDownloadDelete, "#" }
            };

            var managePrnPernCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_ManagePern_Link_CreatePern, UrlHelper.Action(
                    Routes.Actions.PRNS.Create,
                    Routes.Controllers.PRNS) },
                { HomePageResources.HomePage_ManagePern_Link_ViewEditDraftPerns, "#" },
                { HomePageResources.HomePage_ManagePern_Link_ViewSentPerns, UrlHelper.Action(
                    Routes.Actions.PRNS.ViewSentPrns,
                    Routes.Controllers.PRNS) }
            };

            var returnsCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Returns_Link_SubmitReturn, "#" }
            };

            var accreditationCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Accreditation_Link_ApplyForAccreditation, "#" },
                { HomePageResources.HomePage_Accreditation_Link_MyAccreditations, "#" },
                { HomePageResources.HomePage_Accreditation_Link_ViewApplications, "#" }
            };

            var accountCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Account_Link_CreateUser, "#" },
                { HomePageResources.HomePage_Account_Link_ManageUser, "#" },
                { HomePageResources.HomePage_Account_Link_DetailUser, "#" }
            };

            var wasteCardViewModel = GetCardViewModel(HomePageResources.HomePage_Waste_Title, HomePageResources.HomePage_Waste_Description);

            wasteCardViewModel.Links = wasteCardLinks;

            var managePrnPernCardViewModel = GetCardViewModel(HomePageResources.HomePage_ManagePrnPern_Title, HomePageResources.HomePage_ManagePrnPern_Description);
            managePrnPernCardViewModel.Links = managePrnPernCardLinks;

            var returnsCardViewModel = GetCardViewModel(HomePageResources.HomePage_Returns_Title, HomePageResources.HomePage_Returns_Description);
            returnsCardViewModel.Links = returnsCardLinks;

            var accreditationCardViewModel = GetCardViewModel(HomePageResources.HomePage_Accreditation_Title, HomePageResources.HomePage_Accreditation_Description);
            accreditationCardViewModel.Links = accreditationCardLinks;

            var accountCardViewModel = GetCardViewModel(HomePageResources.HomePage_Account_Title, HomePageResources.HomePage_Account_Description);
            accountCardViewModel.Links = accountCardLinks;

            var cardViewModels = new List<CardViewModel>
            {
                wasteCardViewModel,
                managePrnPernCardViewModel,
                returnsCardViewModel,
                accreditationCardViewModel,
                accountCardViewModel
            };

            return cardViewModels;
        }

        public override Task<CYAViewModel> GetCheckAnswers(int journeyId)
        {
            throw new NotImplementedException();
        }
    }
}
