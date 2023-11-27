using Microsoft.AspNetCore.Mvc;
using Portal.Services.Interfaces;
using Portal.ViewModels;

namespace Portal.Controllers
{
    public class WasteController : Controller
    {

        private readonly IWasteService _wasteService;

        public WasteController(
            IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        [HttpGet]
        public async Task<IActionResult> DuringWhichMonth(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var model = await _wasteService.GetCurrentQuarter(id.Value);

            return View(model);
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
        public async Task<IActionResult> WhatHaveYouDoneWaste(int id, WhatHaveYouDoneWasteModel whatHaveYouDoneWaste)
        {
            if (!ModelState.IsValid)
            {
                return View(whatHaveYouDoneWaste);
            }

            await _wasteService.SaveSelectedWasteType(whatHaveYouDoneWaste.JourneyId, whatHaveYouDoneWaste.SelectedWaste);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> DuringWhichMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = await _wasteService.GetCurrentQuarter(duringWhichMonthRequestViewModel.JourneyId);

                return View(model);
            }

            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Types(int? id)
        {
            if (id == null)
                return NotFound();

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

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/WasteRecordStatus")]
        public async Task<IActionResult> GetWasteRecordStatus(int reprocessorId)
        {
            var result = await _wasteService.GetWasteRecordStatus(reprocessorId);
            return View("WasteRecordStatus", result);
        }
    }
}