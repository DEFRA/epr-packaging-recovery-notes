using EPRN.Common.Dtos;
using EPRN.Common.Enums;
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
using System.Globalization;
using static EPRN.Common.Constants.Strings;

namespace EPRN.Portal.Services.HomeServices
{
    public class UserBasedExporterService : UserBasedBaseService, IUserBasedService
    {
        public UserBasedExporterService(IOptions<AppConfigSettings> configSettings, 
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

            var managePernCardLinks = new Dictionary<string, string>()
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

            var managePernCardViewModel = GetCardViewModel(HomePageResources.HomePage_ManagePern_Title, HomePageResources.HomePage_ManagePern_Description);
            managePernCardViewModel.Links = managePernCardLinks;

            var returnsCardViewModel = GetCardViewModel(HomePageResources.HomePage_Returns_Title, HomePageResources.HomePage_Returns_Description);
            returnsCardViewModel.Links = returnsCardLinks;

            var accreditationCardViewModel = GetCardViewModel(HomePageResources.HomePage_Accredidation_Title, HomePageResources.HomePage_Accredidation_Description);
            accreditationCardViewModel.Links = accreditationCardLinks;

            var accountCardViewModel = GetCardViewModel(HomePageResources.HomePage_Account_Title, HomePageResources.HomePage_Account_Description);
            accountCardViewModel.Links = accountCardLinks;

            var cardViewModels = new List<CardViewModel>
            {
                wasteCardViewModel,
                managePernCardViewModel,
                returnsCardViewModel,
                accreditationCardViewModel,
                accountCardViewModel
            };

            return cardViewModels;
        }

        public override double? GetBaledWithWireDeductionPercentage()
        {
            return ConfigSettings.Value.DeductionAmount_Exporter;
        }

        public override async Task<CYAViewModel> GetCheckAnswers(int journeyId)
        {
            var journeyDto = await _httpJourneyService.GetJourneyAnswers(journeyId);

            if (journeyDto == null)
                throw new NullReferenceException(nameof(journeyDto));

            var viewModel = new CYAViewModel
            {
                JourneyId = journeyId,
                Completed = journeyDto.Completed.HasValue ? journeyDto.Completed.Value : false,
                UserRole = UserRole.Exporter
            };

            GetAnswerForWaste(viewModel, journeyDto);

            return viewModel;
        }



        #region check your answers

        private void GetAnswerForWaste(CYAViewModel vm, JourneyAnswersDto journey)
        {
            GetBaseAnswers(vm, journey);

            vm.ReprocessorWhereWasteSentName = string.Empty;
            vm.ReprocessorWhereWasteSentAddress = string.Empty;
        }

        private void GetBaseAnswers(CYAViewModel vm, JourneyAnswersDto journey)
        {
            vm.TypeOfWaste = journey.WasteSubType;
            vm.BaledWithWire = journey.BaledWithWire.HasValue ? (journey.BaledWithWire.Value == true ? "Yes" : "No") : string.Empty;
            vm.TonnageOfWaste = journey.Tonnes.ToString();
            vm.TonnageAdjusted = journey.Adjustment.ToString();
            vm.MonthReceived = journey.Month.HasValue ? (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journey.Month.Value)) : string.Empty;
            vm.Note = journey.Note;
        }

        #endregion
    }
}
