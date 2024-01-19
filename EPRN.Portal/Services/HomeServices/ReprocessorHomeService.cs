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
using NuGet.Packaging;
using static EPRN.Portal.Constants.Strings;

namespace EPRN.Portal.Services.HomeServices
{
    public class ReprocessorHomeService : BaseHomeService, IHomeService
    {
        public ReprocessorHomeService(
            IOptions<AppConfigSettings> configSettings,
            IHttpJourneyService httpJourneyService,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor) : base(configSettings, httpJourneyService, urlHelperFactory, actionContextAccessor)
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

        public override async Task<CheckAnswersViewModel> GetCheckAnswers(int journeyId)
        {
            var journeyDto = await _httpJourneyService.GetJourneyAnswers(journeyId);
            if (journeyDto == null)
                throw new NullReferenceException(nameof(journeyDto));

            var viewModel = new CheckAnswersViewModel { JourneyId = journeyId, Completed = journeyDto.Completed };

            if (journeyDto.WhatDoneWithWaste == null)
                throw new NullReferenceException(nameof(journeyDto.WhatDoneWithWaste));

            var sections = journeyDto.WhatDoneWithWaste == DoneWaste.ReprocessedIt.ToString() ?
                GetAnswerSectionsForWasteReceivedAndReprocessed(journeyDto, journeyId) :
                GetAnswerSectionsForWasteSentOn(journeyDto, journeyId);

            viewModel.Sections.AddRange(sections);
            viewModel.Completed = AreRequiredAnswersProvided(sections[CYAResources.Title]);
            return viewModel;
        }

        private bool AreRequiredAnswersProvided(List<CheckAnswerViewModel> rows)
        {
            bool isValid = true;

            foreach (var item in rows)
            {

                if ((item.Answer == null || item.Answer.Length == 0) && item.Question != CYAResources.Note)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        private Dictionary<string, List<CheckAnswerViewModel>> GetAnswerSectionsForWasteReceivedAndReprocessed(JourneyAnswersDto journey, int journeyId)
        {
            var section = new Dictionary<string, List<CheckAnswerViewModel>>();

            var rows = new List<CheckAnswerViewModel>
            {
                new CheckAnswerViewModel { Question = CYAResources.TypeOfWaste, Answer = journey.WasteSubType, ChangeLink = GenerateUrl(journeyId, "SubTypes") },
                new CheckAnswerViewModel { Question = CYAResources.BaledWithWire, Answer = journey.BaledWithWire, ChangeLink = GenerateUrl(journeyId, "Baled") },
                new CheckAnswerViewModel { Question = CYAResources.TonnageOfWaste, Answer = journey.Tonnes.ToString(), ChangeLink = GenerateUrl(journeyId, "Tonnes") },
                new CheckAnswerViewModel { Question = CYAResources.TonnageAdjusted, Answer = journey.TonnageAdjusted.ToString(), ChangeLink = string.Empty },
                new CheckAnswerViewModel { Question = CYAResources.MonthReceived, Answer = journey.Month, ChangeLink = GenerateUrl(journeyId, "Month") },
                new CheckAnswerViewModel { Question = CYAResources.Note, Answer = journey.Note, ChangeLink = GenerateUrl(journeyId, "Note") }
            };

            section.Add(CYAResources.Title, rows);

            return section;
        }

        private Dictionary<string, List<CheckAnswerViewModel>> GetAnswerSectionsForWasteSentOn(JourneyAnswersDto journey, int journeyId)
        {
            var section = new Dictionary<string, List<CheckAnswerViewModel>>();

            var rows = new List<CheckAnswerViewModel>
            {
                new CheckAnswerViewModel { Question = CYAResources.ReprocessorWhereWasteSentName, Answer = string.Empty, ChangeLink = string.Empty },
                new CheckAnswerViewModel { Question = CYAResources.ReprocessorWhereWasteSentAddress, Answer = string.Empty, ChangeLink = string.Empty },

                new CheckAnswerViewModel { Question = CYAResources.TypeOfWaste, Answer = journey.WasteSubType, ChangeLink = GenerateUrl(journeyId, "SubTypes") },
                new CheckAnswerViewModel { Question = CYAResources.BaledWithWire, Answer = journey.BaledWithWire, ChangeLink = GenerateUrl(journeyId, "Baled") },
                new CheckAnswerViewModel { Question = CYAResources.TonnageOfWaste, Answer = journey.Tonnes.ToString(), ChangeLink = GenerateUrl(journeyId, "Tonnes") },
                new CheckAnswerViewModel { Question = CYAResources.TonnageAdjusted, Answer = journey.TonnageAdjusted.ToString(), ChangeLink = string.Empty },
                new CheckAnswerViewModel { Question = CYAResources.MonthReceived, Answer = journey.Month, ChangeLink = GenerateUrl(journeyId, "Month") },
                new CheckAnswerViewModel { Question = CYAResources.Note, Answer = journey.Note, ChangeLink = GenerateUrl(journeyId, "Note") }
            };

            section.Add(CYAResources.ReprocessorResentPageHeader, rows);

            return section;
        }

        private string GenerateUrl(int journeyId, string actionName)
        {
            if (actionName == "SubTypes")
            {
                return UrlHelper.Action(actionName, "Waste", new { id = journeyId });
            }
            else
            {
                return UrlHelper.Action(actionName, "Waste", new { id = journeyId, rtap = QueryStrings.ReturnToAnswersYes });
            }
        }
    }
}
