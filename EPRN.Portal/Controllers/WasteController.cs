using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EPRN.Portal.Controllers
{
    public class WasteController : Controller
    {
        private readonly IWasteService _wasteService;

        public WasteController(IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        [HttpGet]
        public async Task<IActionResult> WhatHaveYouDoneWaste(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetWasteModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> WhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWaste)
        {
            if (!ModelState.IsValid)
            {
                return View(whatHaveYouDoneWaste);
            }

            await _wasteService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWaste);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Waste/Month/{id}")]
        public async Task<IActionResult> DuringWhichMonth(int? id)
        {
            if (id == null)
                return NotFound();

            int currentMonth = DateTime.Now.Month;

            var model = await _wasteService.GetQuarterForCurrentMonth(id.Value, currentMonth);

            return View(model);
        }

        [HttpPost]
        [Route("Waste/Month/{id}")]
        public async Task<IActionResult> DuringWhichMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                int currentMonth = DateTime.Now.Month;

                var model = await _wasteService.GetQuarterForCurrentMonth(duringWhichMonthRequestViewModel.JourneyId, currentMonth);

                return View(model);
            }

            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This end point creates a new journey and then redirects
        /// to the Types action with the id of the new record id
        /// </summary>
        [HttpGet]
        [Route("Waste/Types")]
        public async Task<IActionResult> Types()
        {
            var journeyId = await _wasteService.CreateJourney();

            return RedirectToAction("Types", new { id = journeyId });
        }

        [HttpGet]
        [Route("Waste/Types/{id:int}")]
        public async Task<IActionResult> Types(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _wasteService.GetWasteTypesViewModel(id.Value);

            return View(viewModel);
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

            return RedirectToAction("Tonnes", "Waste", new { id = wasteSubTypesViewModel.JourneyId });
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

            return RedirectToAction("SubTypes", "Waste", new { id = wasteTypesViewModel.JourneyId });
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

            return RedirectToAction("BaledWithWire", "Waste", new { id = exportTonnageViewModel.JourneyId });
        }

        [HttpGet]
        public async Task<IActionResult> BaledWithWire(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetBaledWithWireModel(id.Value);
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> BaledWithWire(BaledWithWireModel baledWithWireModel)
        {
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
    }
}