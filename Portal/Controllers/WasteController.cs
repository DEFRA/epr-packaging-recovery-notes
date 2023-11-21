using Microsoft.AspNetCore.Mvc;
using Portal.Services.Interfaces;
using Portal.ViewModels.Waste;

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
        public async Task<IActionResult> Type(int? id)
        {
            // in the fullness of time we should add this as a common function
            // in a base controller
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            // retrieve list of waste types from api
            // this should probably go in a cache eventual
            var viewModel = await _wasteService.GetWasteTypeViewModel(id.Value);

            return View(viewModel);
        }

        public async Task<IActionResult> Type(WasteTypeViewModel wasteTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Type(wasteTypeViewModel.JourneyId);
            }

            await _wasteService.SaveSelectedWasteType(wasteTypeViewModel);

            // at the moment we'll redirect to the home page as we don't have the
            // next page to redirect to
            return RedirectToAction("Index", "Home");
        }
    }
}
