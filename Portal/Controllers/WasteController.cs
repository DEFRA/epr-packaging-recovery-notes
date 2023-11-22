using Microsoft.AspNetCore.Mvc;
using Portal.Services.Interfaces;
using Portal.ViewModels;

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
