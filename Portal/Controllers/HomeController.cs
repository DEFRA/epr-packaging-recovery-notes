using Microsoft.AspNetCore.Mvc;
using Portal.ViewModels;
using System.Diagnostics;
using Portal.Models;

namespace Portal.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
        }

        public IActionResult Index()
        {
            var reProcessorViewModel = new ReProcessorSummaryViewModel { Name = "Green LTD", ContactName = "John Watson", AccountNumber = "A/c 1234" };

            return View(reProcessorViewModel);
        }


    }
}