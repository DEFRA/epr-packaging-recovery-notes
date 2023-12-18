using EPRN.Portal.ViewModels.Waste;
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
            var reProcessorViewModel = new ReProcessorSummaryViewModel { Name = "Green LTD", ContactName = "John Watson", AccountNumber = "A/c 1234" };

            return View(reProcessorViewModel);
        }
    }
}