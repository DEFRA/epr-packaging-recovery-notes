using EPRN.Common.Enums;
using EPRN.Portal.Constants;
using EPRN.Portal.Helpers;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EPRN.Portal.Controllers
{
    /// <summary>
    /// The roiute can be navigate here by being either an Exporter or Reprocessor
    /// </summary>
    [TypeFilter(typeof(WasteTypeFilterAttribute))]
    [Route("Waste/{type:WasteType}/[action]/{id?}")]
    public class WasteController : Controller
    {
        private readonly IWasteService _wasteService;
        public WasteType? _wasteType { get; set; }

        private IHomeService _homeService;

        public WasteController(IWasteService wasteService, IHomeServiceFactory homeServiceFactory)
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
            return View("WhatHaveYouDoneWaste", model);
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

            return RedirectToAction("Month", new { id = whatHaveYouDoneWaste.JourneyId, type = _wasteType });
        }

        [HttpGet]
        [ActionName("Month")]
        public async Task<IActionResult> DuringWhichMonth(int? id)
        {
            if (id == null)
                return NotFound();

            int currentMonth = DateTime.Now.Month;

            var model = await _wasteService.GetQuarterForCurrentMonth(id.Value, currentMonth);

            return View("DuringWhichMonth", model);
        }

        [HttpPost]
        [ActionName("Month")]
        public async Task<IActionResult> DuringWhichMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                int currentMonth = DateTime.Now.Month;

                var model = await _wasteService.GetQuarterForCurrentMonth(duringWhichMonthRequestViewModel.JourneyId, currentMonth);

                return View("DuringWhichMonth", model);
            }

            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            return RedirectToAction("SubTypes", new { id = duringWhichMonthRequestViewModel.JourneyId, type = _wasteType });
        }

        [HttpGet]
        public async Task<IActionResult> Types(int? id)
        {
            if (id == null)
            {
                var journeyId = await _wasteService.CreateJourney();

                return RedirectToAction("Types", new { id = journeyId, type = _wasteType });
            }

            var type = _wasteType;

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

            return RedirectToAction("Done", new { id = wasteTypesViewModel.JourneyId, type = _wasteType });
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

            return RedirectToAction("Tonnes", new { id = wasteSubTypesViewModel.JourneyId, type = _wasteType });
        }

        [HttpGet]
        [Route("/Status")]
        public async Task<IActionResult> GetWasteRecordStatus(int journeyId)
        {
            var result = await _wasteService.GetWasteRecordStatus(journeyId);

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

            return RedirectToAction("Baled", new { id = exportTonnageViewModel.JourneyId, type = _wasteType });
        }

        [HttpGet]
        [ActionName("Baled")]
        public async Task<IActionResult> BaledWithWire(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetBaledWithWireModel(id.Value);
            if(model.BaledWithWireDeductionPercentage == null)
                model.BaledWithWireDeductionPercentage = _homeService.GetBaledWithWireDeductionPercentage();

            return View("BaledWithWire", model);
        }

        [HttpPost]
        [ActionName("Baled")]
        public async Task<IActionResult> BaledWithWire(BaledWithWireViewModel baledWithWireModel)
        {
            if (!ModelState.IsValid)
            {
                return View("BaledWithWire", baledWithWireModel);
            }

            await _wasteService.SaveBaledWithWire(baledWithWireModel);
            return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> Note(int? id)
        {
            if (!id.HasValue)
                return NotFound();
            
            var model = await _wasteService.GetNoteViewModel(id.Value);
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Note(NoteViewModel noteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(noteViewModel);
            }

            await _wasteService.SaveNote(noteViewModel);

            return RedirectToAction("Index", "Home");
        }
    }
}