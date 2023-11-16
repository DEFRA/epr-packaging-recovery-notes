using Microsoft.AspNetCore.Mvc;
using Portal.Services.Implementations;

namespace Portal.Controllers
{
    public class DuringWhichMonthController : Controller
    {
        public IActionResult Index()
        {
            var model = new MonthsAvailableService().GetCurrentQuarter();

            return View(model);
        }

        //[HttpPost]
        //public IActionResult Submit()
        //{
            
        //}
    }
}
