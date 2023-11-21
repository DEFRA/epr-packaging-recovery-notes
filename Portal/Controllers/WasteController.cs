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
        public IActionResult DuringWhichMonth(DuringWhichMonthRequestViewModel monthSelected)
        {
            if (!ModelState.IsValid)
            {
                var model = _wasteService.GetCurrentQuarter(monthSelected.JourneyId);

                return View(model);
            }

            //Send monthSelected to API to record for journey
            //Send the journey no to the API
            //Send the month number

            return RedirectToAction("Index", "Home");
        }
    }
}
