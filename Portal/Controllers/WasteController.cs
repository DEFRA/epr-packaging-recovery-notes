using Microsoft.AspNetCore.Mvc;
using Portal.Services.Interfaces;

namespace Portal.Controllers
{
    public class WasteController : Controller
    {
        private readonly IWasteService _wasteService = null;

        public WasteController(
            IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }
        public IActionResult Type(int id)
        {

            return View();
        }
    }
}
