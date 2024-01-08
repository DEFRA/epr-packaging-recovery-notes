using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Areas.Exporter.Controllers
{
    [Area("Exporter")]
    public class PRNSController : Controller
    {
        private IPRNService _prnService;

        public PRNSController(IPRNService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        [HttpGet]
        public async Task<IActionResult> Tonnes(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _prnService.GetTonnesViewModel(id.Value);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Tonnes(TonnesViewModel tonnesViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(tonnesViewModel);
            }

            await _prnService.SaveTonnes(tonnesViewModel);

            return RedirectToAction("Index", "Home");
        }
    }
}
