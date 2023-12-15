using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;

namespace EPRN.Portal.Services
{
    public class HomePageService : IHomePageService
    {
        private readonly IUserRoleService _userRoleService;

        public HomePageService(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService ?? throw new ArgumentNullException(nameof(userRoleService));
        }

        public HomepageViewModel GetHomepageViewModel()
        {
            bool isExporter = _userRoleService.HasRole(UserRole.Exporter);
            bool isReprocessor = _userRoleService.HasRole(UserRole.Reprocessor);
            bool isExporterAndReprocessor = false;

            if (isExporter && isReprocessor)
                isExporterAndReprocessor = true;

            // TODO: Replace with actual data in the future
            var homePageViewModel = new HomepageViewModel
            {
                Name = "Green LTD",
                ContactName = "John Watson",
                AccountNumber = "12 Head office St, Liverpool, L12 345 - 0098678"
            };

            if (isExporterAndReprocessor)
            {
                homePageViewModel.CardViewModels = GetCardViewModels(isExporterAndReprocessor);
            }

            return homePageViewModel;
        }

        private List<CardViewModel> GetCardViewModels(bool isExporterAndReprocessor)
        {
            var wasteCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Waste_Link_RecordWaste, "#" },
                { HomePageResources.HomePage_Waste_Link_ViewEditDownloadDelete, "#" }
            };

            var managePRNPERNCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_ManagePRNEPRN_Link_CreatePRN, "#" },
                { HomePageResources.HomePage_ManagePRNEPRN_Link_ViewEditDraft, "#" },
                { HomePageResources.HomePage_ManagePRNEPRN_Link_ViewSentPRNs, "#" }
            };

            var returnsCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Returns_Link_SubmitReturn, "#" }
            };

            var accreditationCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Accredidation_Link_ApplyForAccredidation, "#" },
                { HomePageResources.HomePage_Accredidation_Link_MyAccredidations, "#" },
                { HomePageResources.HomePage_Accredidation_Link_ViewApplications, "#" }
            };

            var accountCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Account_Link_CreateUser, "#" },
                { HomePageResources.HomePage_Account_Link_ManageUser, "#" },
                { HomePageResources.HomePage_Account_Link_DetailUser, "#" }
            };

            var wasteCardViewModel = new CardViewModel
            {
                Title = HomePageResources.HomePage_Waste_Title,
                Description = HomePageResources.HomePage_Waste_Caption,
                Links = wasteCardLinks
            };

            var managePRNPERNCardViewModel = new CardViewModel
            {
                Title = HomePageResources.HomePage_ManagePRNEPRN_Title,
                Description = HomePageResources.HomePage_ManagePRNEPRN_Caption,
                Links = managePRNPERNCardLinks
            };

            var returnsCardViewModel = new CardViewModel
            {
                Title = HomePageResources.HomePage_Returns_Title,
                Description = HomePageResources.HomePage_Returns_Caption,
                Links = returnsCardLinks
            };

            var accreditationCardViewModel = new CardViewModel
            {
                Title = HomePageResources.HomePage_Accredidation_Title,
                Description = HomePageResources.HomePage_Accredidation_Description,
                Links = accreditationCardLinks
            };

            var accountCardViewModel = new CardViewModel
            {
                Title = HomePageResources.HomePage_Account_Title,
                Description = HomePageResources.HomePage_Account_Description,
                Links = accountCardLinks
            };

            var cardViewModels = new List<CardViewModel>();

            cardViewModels.Add(wasteCardViewModel);

            if (isExporterAndReprocessor)
                cardViewModels.Add(managePRNPERNCardViewModel);

            cardViewModels.Add(returnsCardViewModel);
            cardViewModels.Add(accreditationCardViewModel);
            cardViewModels.Add(accountCardViewModel);

            return cardViewModels;
        }
    }
}
