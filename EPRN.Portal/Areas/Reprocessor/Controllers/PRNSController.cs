using EPRN.Common.Constants;
using EPRN.Common.Enums;
using EPRN.Portal.Controllers;
using EPRN.Portal.Helpers.Extensions;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static EPRN.Common.Constants.Strings;
using static EPRN.Common.Constants.Strings.Routes;

namespace EPRN.Portal.Areas.Reprocessor.Controllers
{
    [Area(Routes.Areas.Reprocessor)]
    public class PRNSController : BaseController
    {
        private readonly IPRNService _prnService;

        private Category Category => Category.Reprocessor;

        public PRNSController(Func<Category, IPRNService> prnServiceFactory)
        {
            if (prnServiceFactory == null)
                throw new ArgumentNullException(nameof(prnServiceFactory));

            var prnService = prnServiceFactory.Invoke(Category);

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
                Routes.Areas.Controllers.Reprocessor.PRNS,
                new 
                { 
                    area = string.Empty 
                });
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{materialId}")]
        public async Task<IActionResult> CreatePrn(int? materialId)
        {
            if (materialId == null)
                return NotFound();

            var prnId = await _prnService.CreatePrnRecord(materialId.Value, Category);

            return RedirectToAction(
                Routes.Areas.Actions.PRNS.DecemberWaste,
                Routes.Areas.Controllers.Reprocessor.PRNS,
                new 
                { 
                    area = Category, 
                    Id = prnId 
                });
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
                            Routes.Areas.Actions.PRNS.DestinationPrn,
                            Routes.Areas.Controllers.Reprocessor.PRNS,
                            new
                            {
                                area = Category,
                                id = checkYourAnswersViewModel.Id
                            });
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

            // if the status is not cancelled, return the cancel view
            if (viewModel.Status != PrnStatus.Cancelled)
                return View(viewModel);
            // otherwise display that the PERN has been canclled
            else
                return View(Routes.Areas.Actions.PRNS.Cancelled, viewModel);
        }

        [HttpPost]
        [ActionName(Routes.Areas.Actions.PRNS.Cancel)]
        public async Task<IActionResult> PRNCancellation(CancelViewModel cancelViewModel)
        {
            if (!ModelState.IsValid)
                return View(cancelViewModel);

            await _prnService.CancelPRN(cancelViewModel);

            // return view to show the PRN has been cancelled
            return View(Routes.Areas.Actions.PRNS.Cancelled, cancelViewModel);
        }

        [HttpGet]
        [ActionName(Routes.Areas.Actions.PRNS.RequestCancel)]
        public async Task<IActionResult> CancelAcceptedPRN(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _prnService.GetRequestCancelViewModel(id.Value);

            // if cancellation requested has not been made
            if (viewModel.Status != PrnStatus.CancellationRequested)
                return View(viewModel);

            // otherwise return a view informing user that the cancellation has
            // now been requested
            return View(
                Routes.Areas.Actions.PRNS.RequestCancelConfirmed,
                viewModel);
        }

        [HttpPost]
        [ActionName(Routes.Areas.Actions.PRNS.RequestCancel)]
        public async Task<IActionResult> CancelAcceptedPRN(RequestCancelViewModel requestCancelViewModel)
        {
            if (!ModelState.IsValid)
                return View(requestCancelViewModel);

            await _prnService.RequestToCancelPRN(requestCancelViewModel);

            return View(
                Routes.Areas.Actions.PRNS.RequestCancelConfirmed, 
                requestCancelViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DecemberWaste(int? id)
        {
            if (id == null)
                return BadRequest();

            var result = await _prnService.GetDecemberWasteModel(id.Value);

            if (result.Item2)
                return View(result.Item1);
            else
                return RedirectToAction(
                    Routes.Areas.Actions.PRNS.Tonnes,
                    new
                    {
                        area = Category,
                        id
                    });
        }

        [HttpPost]
        public async Task<IActionResult> DecemberWaste(DecemberWasteViewModel decemberWaste)
        {
            if (!ModelState.IsValid)
                return View(decemberWaste);

            await _prnService.SaveDecemberWaste(decemberWaste);

            return RedirectToAction(
                Routes.Areas.Actions.PRNS.Tonnes, 
                new 
                { 
                    area = Category, 
                    id = decemberWaste.Id 
                });
        }

        [HttpGet]
        public async Task<IActionResult> DestinationPrn(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _prnService.GetActionPrnViewModel(id.Value);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DestinationPrn(DestinationPrnViewModel destinationPrnViewModel)
        {
            if (!ModelState.IsValid)
                return View(destinationPrnViewModel);

            if (destinationPrnViewModel.DoWithPRN == PrnStatus.Draft)
            {
                await _prnService.SaveDraftPrn(destinationPrnViewModel);
                return RedirectToAction(Routes.Areas.Actions.PRNS.PrnSavedAsDraftConfirmation,
                                new { area = Category.ToString(), destinationPrnViewModel.Id });
            }
            else
                return RedirectToAction(Routes.Areas.Actions.PRNS.Confirmation, new { area = Category, destinationPrnViewModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> PrnSavedAsDraftConfirmation(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await _prnService.GetDraftPrnConfirmationModel(id.Value);

            return View(viewModel);
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