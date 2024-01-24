using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EPRN.Portal.Controllers
{
    /// <summary>
    /// The roiute can be navigate here by being either an Exporter or Reprocessor
    /// </summary>
    [Route("[controller]/[action]/{id?}")]
    public class WasteController : BaseController
    {
        private readonly IWasteService _wasteService;
        private IUserBasedService _homeService;

        public WasteController(
            IWasteService wasteService,
            IHomeServiceFactory homeServiceFactory)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));

            if (homeServiceFactory == null) throw new ArgumentNullException(nameof(homeServiceFactory));
            _homeService = homeServiceFactory.CreateHomeService();
            if (_homeService == null) throw new ArgumentNullException(nameof(_homeService));
        }

        [HttpGet]
        [ActionName("Done")]
        public async Task<IActionResult> WhatHaveYouDoneWaste(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetWasteModel(id.Value);
            return View(model);
        }

        [HttpPost]
        [ActionName("Done")]
        public async Task<IActionResult> WhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWaste)
        {
            if (!ModelState.IsValid)
            {
                return View("WhatHaveYouDoneWaste", whatHaveYouDoneWaste);
            }

            await _wasteService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWaste);

            return RedirectToAction("Month", new { id = whatHaveYouDoneWaste.JourneyId });
            //return RedirectToPage("Month", whatHaveYouDoneWaste.JourneyId);
        }

        [HttpGet]
        [ActionName("Month")]
        public async Task<IActionResult> DuringWhichMonth(int? id)
        {
            if (id == null)
                return NotFound();

            int currentMonth = DateTime.Now.Month;

            var model = await _wasteService.GetQuarterForCurrentMonth(id.Value, currentMonth);

            return View(model);
        }

        [HttpPost]
        [ActionName("Month")]
        public async Task<IActionResult> DuringWhichMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                int currentMonth = DateTime.Now.Month;

                var model = await _wasteService.GetQuarterForCurrentMonth(duringWhichMonthRequestViewModel.JourneyId, currentMonth);

                return View(model);
            }

            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            return RedirectToAction("SubTypes", new { id = duringWhichMonthRequestViewModel.JourneyId });
            //return RedirectToPage("SubTypes", duringWhichMonthRequestViewModel.JourneyId);
        }

        [HttpGet]
        public async Task<IActionResult> Types(int? id)
        {
            if (id == null)
            {
                // TODO - need to record the type of waste (Exporter or Reprocessor)
                // but we're limited here as the Types page needs re-doing due to a change
                // of design
                var journeyId = await _wasteService.CreateJourney();

                return RedirectToAction("Types", new { id = journeyId });
            }

            var viewModel = await _wasteService.GetWasteTypesViewModel(id.Value);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Types(WasteTypesViewModel wasteTypesViewModel)
        {
            if (wasteTypesViewModel == null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return await Types(wasteTypesViewModel.JourneyId);
            }

            await _wasteService.SaveSelectedWasteType(wasteTypesViewModel);

            return RedirectToAction("Done", new { id = wasteTypesViewModel.JourneyId });
        }

        [HttpGet]
        public async Task<IActionResult> SubTypes(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _wasteService.GetWasteSubTypesViewModel(id.Value);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubTypes(WasteSubTypesViewModel wasteSubTypesViewModel)
        {
            if (wasteSubTypesViewModel == null)
                return BadRequest();

            if (!ModelState.IsValid &&
                wasteSubTypesViewModel.AdjustmentRequired &&
                ModelState["CustomPercentage"].ValidationState == ModelValidationState.Invalid)
            {
                return await SubTypes(wasteSubTypesViewModel.JourneyId);
            }

            await _wasteService.SaveSelectedWasteSubType(wasteSubTypesViewModel);

            return RedirectToAction("Tonnes", new { id = wasteSubTypesViewModel.JourneyId });
        }

        [HttpGet]
        public async Task<IActionResult> GetWasteRecordStatus(int? id)
        {
            if (id == null)
                return NotFound();

            var result = await _wasteService.GetWasteRecordStatus(id.Value);

            if (result.WasteRecordStatus == WasteRecordStatuses.Complete)
                return View("WasteRecordCompleteStatus", result);

            return View("WasteRecordStatus", result);
        }

        [HttpGet]
        public async Task<IActionResult> Tonnes(int? id)
        {
            if (id == null)
                return NotFound();

            var exportTonnageViewModel = await _wasteService.GetExportTonnageViewModel(id.Value);

            return View(exportTonnageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Tonnes(ExportTonnageViewModel exportTonnageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(exportTonnageViewModel);
            }

            await _wasteService.SaveTonnage(exportTonnageViewModel);

            return RedirectToAction("Baled", new { id = exportTonnageViewModel.JourneyId });
        }

        [HttpGet]
        [ActionName("Baled")]
        public async Task<IActionResult> BaledWithWire(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetBaledWithWireModel(id.Value);
            if (model.BaledWithWireDeductionPercentage == null)
                model.BaledWithWireDeductionPercentage = _homeService.GetBaledWithWireDeductionPercentage();

            return View(model);
        }

        [HttpPost]
        [ActionName("Baled")]
        public async Task<IActionResult> BaledWithWire(BaledWithWireViewModel baledWithWireModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Baled", baledWithWireModel);
            }

            await _wasteService.SaveBaledWithWire(baledWithWireModel);

            return RedirectToAction("Note", new { id = baledWithWireModel.JourneyId });
        }

        [HttpGet]
        public async Task<IActionResult> ReProcessorExport(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetReProcessorExportViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReProcessorExport(ReProcessorExportViewModel reProcessorExportViewModel)
        {
            await _wasteService.SaveReprocessorExport(reProcessorExportViewModel);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [ActionName("Note")]
        public async Task<IActionResult> Note(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _wasteService.GetNoteViewModel(id.Value);

            return View(model);
        }

        [HttpPost]
        [ActionName("Note")]
        public async Task<IActionResult> Note(NoteViewModel noteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(noteViewModel);
            }

            await _wasteService.SaveNote(noteViewModel);

            // if qs contains certain value then redirect to answers page
            // else continue
            return RedirectToAction("Index", "Home");    
            //return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CheckYourAnswers(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var vm = await _homeService.GetCheckAnswers(id.Value);

            switch (vm.UserRole)
            {
                case UserRole.None:
                    throw new Exception("Could not determine user role");

                case UserRole.Exporter:
                    return View("CYAExporter", vm);

                case UserRole.Reprocessor:
                    {
                        if (vm.DoneWaste == DoneWaste.ReprocessedIt)
                            return View("CYAReprocessorReprocessedIt", vm);
                        else if (vm.DoneWaste == DoneWaste.SentItOn)
                            return View("CYAReprocessorSentItOn", vm);
                        else
                            throw new Exception("Could not determine what happened to the waste");
                    }

                default:
                    throw new Exception("Could not determine user role");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckYourAnswers(CYAViewModel checkAnswersViewModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _homeService.GetCheckAnswers(checkAnswersViewModel.JourneyId);
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Handle redirection to CheckYourAnswers if this is where we originally came from
            if (context.HttpContext.Request.Query.ContainsKey(Constants.Strings.QueryStrings.ReturnToAnswers) &&
                context.HttpContext.Request.Query[Constants.Strings.QueryStrings.ReturnToAnswers] == Constants.Strings.QueryStrings.ReturnToAnswersYes &&
                context.Result is RedirectToActionResult)
            {
                var id = (context.Result as RedirectToActionResult).RouteValues["Id"].ToString();
                context.Result = RedirectToAction("CheckYourAnswers", "Waste", new { id });
            }

            base.OnActionExecuted(context);
        }
    }
}