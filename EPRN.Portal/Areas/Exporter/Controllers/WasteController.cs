using EPRN.Portal.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Areas.Exporter.Controllers
{
    [Area("Exporter")]
    public class WasteController : BaseController
    {
        private readonly IWasteService _wasteService;

        public WasteController(IWasteService wasteService)
        {
            _wasteService = wasteService;
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        [HttpGet]
        [ActionName("Month")]
        public async Task<IActionResult> DuringWhichMonth(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _wasteService.GetQuarterForCurrentMonth(id.Value);

            return View(model);
        }

        [HttpPost]
        [ActionName("Month")]
        public async Task<IActionResult> DuringWhichMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (!ModelState.IsValid)
            {

                var model = await _wasteService.GetQuarterForCurrentMonth(duringWhichMonthRequestViewModel.Id);

                return View(model);
            }

            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            return RedirectToAction("SubTypes", new { id = duringWhichMonthRequestViewModel.Id });
        }
    }
}