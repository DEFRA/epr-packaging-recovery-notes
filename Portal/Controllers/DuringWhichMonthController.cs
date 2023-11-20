using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using Portal.Services.Implementations;

namespace Portal.Controllers
{
    public class DuringWhichMonthController : Controller
    {
        [HttpGet]
        public IActionResult Index(int id)
        {
            var model = new WasteService().GetCurrentQuarter();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(DuringWhichMonthRequestViewModel monthSelected)
        {
            if (!ModelState.IsValid)
            {
                var model = new WasteService().GetCurrentQuarter();

                return View(model);
            }

            //Send monthSelected to API to record for journey

            return RedirectToAction("Index", "Home");
        }
    }
}