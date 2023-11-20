using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using Portal.Services.Implementations;

namespace Portal.Controllers
{
    public class WasteController : Controller
    {
        [HttpGet]
        public IActionResult DuringWhichMonth(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var model = new WasteService().GetCurrentQuarter(id.Value);

            return View(model);
        }

        [HttpPost]
        public IActionResult DuringWhichMonth(DuringWhichMonthRequestViewModel monthSelected)
        {
            if (!ModelState.IsValid)
            {
                var model = new WasteService().GetCurrentQuarter(monthSelected.JourneyId);

                return View(model);
            }

            //Send monthSelected to API to record for journey
            //Send the journey no to the API
            //Send the month number

            return RedirectToAction("Index", "Home");
        }
    }
}
