﻿using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Filters;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Routes = EPRN.Common.Constants.Strings.Routes;

namespace EPRN.Portal.Controllers
{
    /// <summary>
    /// The roiute can be navigate here by being either an Exporter or Reprocessor
    /// </summary>
    [Route("[controller]/[action]/{id?}")]
    [ServiceFilter(typeof(WasteTypeActionFilter))]
    public class WasteController : BaseController
    {
        private readonly IWasteService _wasteService;
        private IHomeService _homeService;

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
        [ActionName(Routes.Actions.Waste.Done)]
        public async Task<IActionResult> WhatHaveYouDoneWaste(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetWasteModel(id.Value);
            return View(model);
        }

        [HttpPost]
        [ActionName(Routes.Actions.Waste.Done)]
        public async Task<IActionResult> WhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWaste)
        {
            if (!ModelState.IsValid)
                return View(whatHaveYouDoneWaste);

            await _wasteService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWaste);

            return RedirectToAction(
                Routes.Actions.Waste.Month, 
                new { id = whatHaveYouDoneWaste.JourneyId });
        }

        [HttpGet]
        [ActionName(Routes.Actions.Waste.Month)]
        public async Task<IActionResult> DuringWhichMonth(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetQuarterForCurrentMonth(id.Value);

            return View(model);
        }

        [HttpPost]
        [ActionName(Routes.Actions.Waste.Month)]
        public async Task<IActionResult> DuringWhichMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = await _wasteService.GetQuarterForCurrentMonth(duringWhichMonthRequestViewModel.JourneyId);

                return View(model);
            }

            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            return RedirectToAction(
                Routes.Actions.Waste.SubTypes, 
                new { id = duringWhichMonthRequestViewModel.JourneyId });
        }

        /// <summary>
        /// Displays the Record Waste view
        /// Dual usage - 
        /// if no ID is provided then it is for creating a new record
        /// if an ID for an existing record is provided then it is to edit the material type
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RecordWaste(int? id)
        {
            var viewModel = await _wasteService.GetWasteTypesViewModel(id);

            return View(viewModel);
        }

        /// <summary>
        /// Record change to material type for existing waste record
        /// </summary>
        [HttpGet]
        [Route("/[controller]/[action]/{id}/Material/{materialId}")]
        public async Task<IActionResult> RecordWaste(WasteTypeViewModel wasteTypesViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _wasteService.SaveSelectedWasteType(wasteTypesViewModel);

            return RedirectToAction(
                Routes.Actions.Waste.Done,
                new { id = wasteTypesViewModel.Id });
        }

        /// <summary>
        /// Creates a new Waste Record. Requires meterial ID and Category (Exporter or Reprocessor)
        /// </summary>
        [HttpGet]
        [Route("/[controller]/[action]/Material/{materialId}/Category/{category}")]
        public async Task<IActionResult> Create(
            int? materialId, 
            Category? category)
        {
            if (materialId == null)
                return BadRequest();

            if (category == null)
                return BadRequest();

            var id = await _wasteService.CreateJourney(
                materialId.Value,
                category.Value);

            return RedirectToAction(
                Routes.Actions.Waste.Done, 
                new { id });
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

            return RedirectToAction(
                Routes.Actions.Waste.Tonnes, 
                new { id = wasteSubTypesViewModel.JourneyId });
        }

        [HttpGet]
        public async Task<IActionResult> WasteRecordStatus(int? id)
        {
            if (id == null)
                return NotFound();

            var result = await _wasteService.GetWasteRecordStatus(id.Value);

            if (result.WasteRecordStatus == WasteRecordStatuses.Complete)
                return View("WasteRecordCompleteStatus", result);

            return View(result);
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

            return RedirectToAction(
                Routes.Actions.Waste.Baled, 
                new { id = exportTonnageViewModel.JourneyId });
        }

        [HttpGet]
        [ActionName(Routes.Actions.Waste.Baled)]
        public async Task<IActionResult> BaledWithWire(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetBaledWithWireModel(id.Value);
            if(model.BaledWithWireDeductionPercentage == null)
                model.BaledWithWireDeductionPercentage = _homeService.GetBaledWithWireDeductionPercentage();

            return View(model);
        }

        [HttpPost]
        [ActionName(Routes.Actions.Waste.Baled)]
        public async Task<IActionResult> BaledWithWire(BaledWithWireViewModel baledWithWireModel)
        {
            if (!ModelState.IsValid)
            {
                return View(baledWithWireModel);
            }

            await _wasteService.SaveBaledWithWire(baledWithWireModel);
            return RedirectToAction(
                Routes.Actions.Waste.Note, 
                new { id = baledWithWireModel.JourneyId });
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
        [ActionName(Routes.Actions.Waste.Note)]
        public async Task<IActionResult> Note(int? id)
        {
            if (!id.HasValue)
                return NotFound();
            
            var model = await _wasteService.GetNoteViewModel(id.Value);
            
            return View(model);
        }

        [HttpPost]
        [ActionName(Routes.Actions.Waste.Note)]
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