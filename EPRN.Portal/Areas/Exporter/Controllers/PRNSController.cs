using EPRN.Common.Constants;
using EPRN.Common.Enums;
using EPRN.Portal.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static EPRN.Common.Constants.Strings;

namespace EPRN.Portal.Areas.Exporter.Controllers
{
    [Area(Routes.Areas.Exporter)]
    public class PRNSController : BaseController
    {
        private readonly IPRNService _prnService;

        private Category Category => Category.Exporter;

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

            return RedirectToAction(
                Routes.Areas.Actions.PRNS.SentTo, 
                Routes.Areas.Controllers.Exporter.PRNS, 
                new { area = Category, tonnesViewModel.Id });
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{materialId}")]
        public async Task<IActionResult> CreatePrn(int? materialId)
        {
            if (materialId == null)
                return NotFound();

            var prnId = await _prnService.CreatePrnRecord(materialId.Value, Category);

            return RedirectToAction(
                Routes.Areas.Actions.PRNS.Tonnes,
                Routes.Areas.Controllers.Exporter.PRNS,
                new { Id = prnId });
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int? id)
        {
            if (id == null)
                return NotFound();

            var confirmation = await _prnService.GetConfirmation(id.Value);
            return View(confirmation);
        }

        [HttpGet]
        public async Task<IActionResult> CheckYourAnswers(int? id)
        {
            if (id == null)
                return NotFound();

            var checkYourAnswers = await _prnService.GetCheckYourAnswersViewModel(id.Value);

            return View(checkYourAnswers);
        }

        [HttpPost]
        public async Task<IActionResult> CheckYourAnswers(CheckYourAnswersViewModel checkYourAnswersViewModel)
        {
            // No need for a check on ModelState as there are no attributes on it
            // if there is an issoe in the contents of the model, then a BadRequest will
            // handle this through the MVC framework

            await _prnService.SaveCheckYourAnswers(checkYourAnswersViewModel.Id);

            return RedirectToAction(
                Routes.Areas.Actions.PRNS.WhatToDo,
                Routes.Areas.Controllers.Exporter.PRNS, 
                new { area = Category.ToString(), id = checkYourAnswersViewModel.Id });
        }

        // TODO This is for story #280981 Which packaging producer or compliance scheme is this for? 
        // I needed to stub this for check your answers so that it knows where to redirect to
        // remove this comment when whoever it is devs it
        [HttpGet]
        public async Task<IActionResult> SentTo(int? id)
        {
            return View();
        }

        // TODO This is for story #282464 which is "What do you want to do with this PRN?"
        // stubbed so that the action exists for completinig CheckYourAnswers
        // remove this comment when whoever it is devs it
        [HttpGet]
        public async Task<IActionResult> WhatToDo(int? id)
        {
            return View();
        }

        [HttpGet]
        [ActionName(Routes.Areas.Actions.PRNS.Cancel)]
        public async Task<IActionResult> PRNCancellation(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _prnService.GetCancelViewModel(id.Value);
            return View(viewModel);
        }

        [HttpPost]
        [ActionName(Routes.Areas.Actions.PRNS.Cancel)]
        public async Task<IActionResult> PRNCancellation(CancelViewModel cancelViewModel)
        {
            if (!ModelState.IsValid)
                return View(await _prnService.GetCancelViewModel(cancelViewModel.Id));

            await _prnService.CancelPRN(cancelViewModel);

            return RedirectToAction(
                Routes.Areas.Actions.PRNS.Cancelled,
                Routes.Areas.Controllers.Exporter.PRNS,
                new { area = Routes.Areas.Exporter, id = cancelViewModel.Id });
        }

        [HttpGet]
        [ActionName(Routes.Areas.Actions.PRNS.RequestCancel)]
        public async Task<IActionResult> RequestCancellation(int? id)
        {
            // *** Stubbed method ***
            /*
             * For a user to request from the recipient to Cancel the PRN
             */
            return null;
        }

        [HttpGet]
        public IActionResult Cancelled(int? id)
        {
            // *** Stubbed method ***
            return View();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Handle redirection to CheckYourAnswers if this is where we originally came from
            if (context.HttpContext.Request.Query.ContainsKey(Strings.QueryStrings.ReturnToAnswers) &&
                context.HttpContext.Request.Query[Strings.QueryStrings.ReturnToAnswers] == Strings.QueryStrings.ReturnToAnswersYes &&
                context.Result is RedirectToActionResult)
            {
                var id = (context.Result as RedirectToActionResult).RouteValues["Id"].ToString();
                context.Result = RedirectToAction("CheckYourAnswers", "PRNS", new { area = Category.ToString(), id });
            }

            base.OnActionExecuted(context);
        }
    }
}