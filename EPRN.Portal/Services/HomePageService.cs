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

            // TODO: Replace with actual data in the future
            var homePageViewModel = new HomepageViewModel
            {
                Name = "Green LTD",
                ContactName = "John Watson",
                AccountNumber = "12 Head office St, Liverpool, L12 345 - 0098678"
            };

            homePageViewModel.CardViewModels = GetCardViewModels(isExporter, isReprocessor);

            return homePageViewModel;
        }

        private CardViewModel GetCardViewModel(string title, string description)
        {
            var cardViewModel = new CardViewModel
            {
                Title = title,
                Description = description
            };

            return cardViewModel;
        }

        private List<CardViewModel> GetCardViewModels(bool isExporter, bool isReprocessor)
        {
            var wasteCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_Waste_Link_RecordWaste, "#" },
                { HomePageResources.HomePage_Waste_Link_ViewEditDownloadDelete, "#" }
            };

            var managePrnCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_ManagePrn_Link_CreatePrn, "#" },
                { HomePageResources.HomePage_ManagePrn_Link_ViewEditDraftPrn, "#" },
                { HomePageResources.HomePage_ManagePrn_Link_ViewSentPrns, "#" }
            };

            var managePernCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_ManagePern_Link_CreatePern, "#" },
                { HomePageResources.HomePage_ManagePern_Link_ViewEditDraftPerns, "#" },
                { HomePageResources.HomePage_ManagePern_Link_ViewSentPerns, "#" }
            };

            var managePrnPernCardLinks = new Dictionary<string, string>()
            {
                { HomePageResources.HomePage_ManagePrnPern_Link_CreatePrnPern, "#" },
                { HomePageResources.HomePage_ManagePrnPern_Link_ViewEditDraftPrnsPerns, "#" },
                { HomePageResources.HomePage_ManagePrnPern_Link_ViewSentPrnPerns, "#" }
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

            var wasteCardViewModel = GetCardViewModel(HomePageResources.HomePage_Waste_Title, HomePageResources.HomePage_Waste_Description);

            wasteCardViewModel.Links = wasteCardLinks;

            var managePrnCardViewModel = GetCardViewModel(HomePageResources.HomePage_ManagePrn_Title, HomePageResources.HomePage_ManagePrn_Description);
            managePrnCardViewModel.Links = managePrnCardLinks;

            var managePernCardViewModel = GetCardViewModel(HomePageResources.HomePage_ManagePern_Title, HomePageResources.HomePage_ManagePern_Description);
            managePernCardViewModel.Links = managePernCardLinks;

            var managePrnPernCardViewModel = GetCardViewModel(HomePageResources.HomePage_ManagePrnPern_Title, HomePageResources.HomePage_ManagePrnPern_Description);
            managePrnPernCardViewModel.Links = managePrnPernCardLinks;

            var returnsCardViewModel = GetCardViewModel(HomePageResources.HomePage_Returns_Title, HomePageResources.HomePage_Returns_Description);
            returnsCardViewModel.Links = returnsCardLinks;

            var accreditationCardViewModel = GetCardViewModel(HomePageResources.HomePage_Accredidation_Title, HomePageResources.HomePage_Accredidation_Description);
            accreditationCardViewModel.Links = accreditationCardLinks;

            var accountCardViewModel = GetCardViewModel(HomePageResources.HomePage_Account_Title, HomePageResources.HomePage_Account_Description);
            accountCardViewModel.Links = accountCardLinks;

            var cardViewModels = new List<CardViewModel>();

            cardViewModels.Add(wasteCardViewModel);

            switch (isExporter, isReprocessor)
            {
                case (true, false):
                    cardViewModels.Add(managePernCardViewModel);
                    break;
                case (false, true):
                    cardViewModels.Add(managePrnCardViewModel);
                    break;
                case (true, true):
                    cardViewModels.Add(managePrnPernCardViewModel);
                    break;
            }

            cardViewModels.Add(returnsCardViewModel);
            cardViewModels.Add(accreditationCardViewModel);
            cardViewModels.Add(accountCardViewModel);

            return cardViewModels;
        }
    }
}
