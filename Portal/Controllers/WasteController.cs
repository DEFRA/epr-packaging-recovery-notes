using Microsoft.AspNetCore.Mvc;
using PRN.Web.Models;
using System.Diagnostics;

namespace PRN.Web.Controllers
{
    public class WasteController : Controller
    {
        private readonly ILogger<WasteController> _logger;

        public WasteController(ILogger<WasteController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // return view 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}