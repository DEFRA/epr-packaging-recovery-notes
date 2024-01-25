using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using static EPRN.Common.Constants.Strings;

namespace EPRN.Portal.Controllers
{
    public class PrnsController : BaseController
    {
        private readonly IPRNService _prnService;

        public PrnsController(IPRNService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        public async Task<IActionResult> Create(int? id)
        {
            var viewModel = await _prnService.CreatePrnViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PrnSavedAsDraftConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else if (id <= 0)
            {
                return BadRequest();
            }

            var viewModel = await _prnService.GetDraftPrnConfirmationModel(id.Value);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Action(int? id)
        {
            if (id == null)
                return NotFound();
     
            var viewModel = await _prnService.GetActionPrnViewModel(id.Value);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Action(ActionPrnViewModel actionPrnViewModel)
        {
            if (actionPrnViewModel.DoWithPRN == Common.Enums.PrnStatus.Draft)
                return RedirectToAction(Routes.Actions.Prns.PrnSavedAsDraftConfirmation, new { id = actionPrnViewModel.Id });
            else
                return RedirectToAction(Routes.Areas.Actions.PRNS.Confirmation, new { area = Category.Exporter, actionPrnViewModel.Id });
        }
    }
}