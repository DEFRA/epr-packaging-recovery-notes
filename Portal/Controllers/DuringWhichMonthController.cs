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
            //Send monthSelected to API to record for journey

            if (!ModelState.IsValid)
            {
                var model = new WasteService().GetCurrentQuarter();

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

//ASP.NET Model binding and valdidation