using Microsoft.AspNetCore.Mvc;
using Portal.Services.Interfaces;
using Portal.ViewModels;
using System.Reflection;

namespace Portal.Controllers
{
    public class WasteController : Controller
    {

        private readonly IWasteService _wasteService;

        public WasteController(IWasteService wasteService)
        {
            _wasteService = wasteService;
        }

        [HttpGet]
        public IActionResult DuringWhichMonth(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var model = _wasteService.GetCurrentQuarter(id.Value);

            return View(model);
        }

        [HttpGet]
        public IActionResult Index(int? id)
        {
            if (id == null)
                id = 1;
                //throw new ArgumentNullException("id");

            var model = _wasteService.GetWasteModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int id, WhatHaveYouDoneWasteModel whatHaveYouDoneWaste) 
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
                var model = _wasteService.GetCurrentQuarter(duringWhichMonthRequestViewModel.JourneyId);

                return View(model);
            }

            //Send monthSelected to API to record 
            //Send the journey no to the API
            //Send the month number

            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel.JourneyId, duringWhichMonthRequestViewModel.SelectedMonth.Value);

            return RedirectToAction("Index", "Home");
        }
    }
}