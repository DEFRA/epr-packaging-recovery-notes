using Microsoft.AspNetCore.Mvc;

namespace Portal.Controllers.Areas.Waste
{
    [Area("Waste")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
