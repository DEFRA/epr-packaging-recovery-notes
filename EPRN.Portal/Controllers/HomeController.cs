using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger)
        {
        }

        public IActionResult Index()
        {
            var reProcessorViewModel = new ReProcessorSummaryViewModel { Name = "Green LTD", ContactName = "John Watson", AccountNumber = "12 Head office St, Liverpool, L12 345 - 0098678" };

            return View(reProcessorViewModel);
        }


    }
}