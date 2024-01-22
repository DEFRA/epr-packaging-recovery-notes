using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Configuration;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.Extensions.Options;

namespace EPRN.Portal.Services.HomeServices
{
    public class UserBasedReprocessorService : UserBasedBaseService, IUserBasedService
    {
        public UserBasedReprocessorService(
            IOptions<AppConfigSettings> configSettings,
            IHttpJourneyService httpJourneyService) : base(configSettings, httpJourneyService)
        {
        }

        protected override List<CardViewModel> GetCardViewModels()
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

            var returnsCardViewModel = GetCardViewModel(HomePageResources.HomePage_Returns_Title, HomePageResources.HomePage_Returns_Description);
            returnsCardViewModel.Links = returnsCardLinks;

            var accreditationCardViewModel = GetCardViewModel(HomePageResources.HomePage_Accredidation_Title, HomePageResources.HomePage_Accredidation_Description);
            accreditationCardViewModel.Links = accreditationCardLinks;

            var accountCardViewModel = GetCardViewModel(HomePageResources.HomePage_Account_Title, HomePageResources.HomePage_Account_Description);
            accountCardViewModel.Links = accountCardLinks;

            var cardViewModels = new List<CardViewModel>
            {
                wasteCardViewModel,
                managePrnCardViewModel,
                returnsCardViewModel,
                accreditationCardViewModel,
                accountCardViewModel
            };

            return cardViewModels;
        }

        public override double? GetBaledWithWireDeductionPercentage()
        {
            return ConfigSettings.Value.DeductionAmount_Reprocessor;
        }

        public override async Task<CYAReprocessorViewModel> GetCheckAnswers(int journeyId)
        {
            var journeyDto = await _httpJourneyService.GetJourneyAnswers(journeyId);

            if (journeyDto == null)
                throw new NullReferenceException(nameof(journeyDto));

            if (journeyDto.WhatDoneWithWaste == null || !Enum.TryParse(journeyDto.WhatDoneWithWaste, out DoneWaste doneWaste))
                throw new NullReferenceException(nameof(journeyDto.WhatDoneWithWaste));

            var viewModel = new CYAReprocessorViewModel { JourneyId = journeyId, 
                Completed = journeyDto.Completed, 
                DoneWaste = doneWaste 
            };

            if (journeyDto.WhatDoneWithWaste == DoneWaste.ReprocessedIt.ToString())
                GetAnswerForWasteReceivedAndReprocessed(viewModel, journeyDto);
            else
                GetAnswerForWasteSentOn(viewModel, journeyDto);

            return viewModel;
        }


        #region check your answers

        private void GetAnswerForWasteReceivedAndReprocessed(CYAReprocessorViewModel vm, JourneyAnswersDto journey)
        {
            GetBaseAnswers(vm, journey);
        }

        private void GetAnswerForWasteSentOn(CYAReprocessorViewModel vm, JourneyAnswersDto journey)
        {
            GetBaseAnswers(vm, journey);

            vm.ReprocessorWhereWasteSentName = string.Empty;
            vm.ReprocessorWhereWasteSentAddress = string.Empty;
        }

        private void GetBaseAnswers(CYAReprocessorViewModel vm, JourneyAnswersDto journey)
        {
            vm.TypeOfWaste = journey.WasteSubType;
            vm.BaledWithWire = journey.BaledWithWire;
            vm.TonnageOfWaste = journey.Tonnes.ToString();
            vm.TonnageAdjusted = journey.TonnageAdjusted.ToString();
            vm.MonthReceived = journey.Month;
            vm.Note = journey.Note;
        }

        #endregion

    }
}
