using EPRN.Common.Constants;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using static EPRN.Common.Constants.Strings;
using Routes = EPRN.Common.Constants.Strings.Routes;

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
                return NotFound();
            
            var viewModel = await _prnService.GetDraftPrnConfirmationModel(id.Value);

            return View(viewModel);
        }

        [HttpGet]
        [ActionName(Routes.Actions.PRNS.View)]
        [Route("[controller]/[action]/{reference}")]
        public async Task<IActionResult> ViewPRN(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                return NotFound();

            var viewModel = await _prnService.GetViewPrnViewModel(reference);

            return View(viewModel);
        }

        [HttpGet]
        [ActionName(Routes.Actions.PRNS.ViewSentPrns)]
        public async Task<IActionResult> ViewSentPrns([FromQuery] GetSentPrnsViewModel request)
        {
            var viewSentPrnsViewModel = await _prnService.GetViewSentPrnsViewModel(request);

            return View(viewSentPrnsViewModel);
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
        public IActionResult Action(ActionPrnViewModel actionPrnViewModel)
        {
            if (!ModelState.IsValid)
                return View(actionPrnViewModel);

            if (actionPrnViewModel.DoWithPRN == Common.Enums.PrnStatus.Draft)
                return RedirectToAction(Routes.Actions.PRNS.PrnSavedAsDraftConfirmation, new { id = actionPrnViewModel.Id });
            else
                return RedirectToAction(Routes.Areas.Actions.PRNS.Confirmation, new { area = Category.Exporter, actionPrnViewModel.Id });
        }
    }
}