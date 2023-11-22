using Microsoft.AspNetCore.Mvc;
using Portal.Services.Interfaces;
using Portal.ViewModels;

namespace Portal.Controllers
{
    public class WasteController : Controller
    {
        private readonly IWasteService _wasteService;

        public WasteController(
            IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        [HttpGet]
        public async Task<IActionResult> Types(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _wasteService.GetWasteTypesViewModel(id.Value);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Types(WasteTypesViewModel wasteTypes)
        {
            if (wasteTypes == null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return await Types(wasteTypes.JourneyId);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
