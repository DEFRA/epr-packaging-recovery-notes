using EPRN.Common.Enums;
using EPRN.Portal.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;

namespace EPRN.Portal.Areas.Reprocessor.Controllers
{
    [Area("Reprocessor")]
    public class PRNSController : BaseController
    {
        private IPRNService _prnService;

        private Category Category => Category.Reprocessor;

        public PRNSController(Func<Category, IPRNService> prnServiceFactory)
        {
            if (prnServiceFactory == null)
                throw new ArgumentNullException(nameof(prnServiceFactory));

            _prnService = prnServiceFactory.Invoke(Category);
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

            return RedirectToAction("Create", "PRNS", new { area = string.Empty });
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{materialId}")]
        public async Task<IActionResult> CreatePrn(int? materialId)
        {
            if (materialId == null)
                return NotFound();

            var prnId = await _prnService.CreatePrnRecord(materialId.Value);

            return RedirectToAction("Tonnes", "PRNS", new { Id = prnId });
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int? id)
        {
            if (id == null)
                return NotFound();

            var confirmation = await _prnService.GetConfirmation(id.Value);

            return View(confirmation);
        }
    }
}
