using EPRN.Common.Enums;
using EPRN.Portal.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.Portal.Areas.Exporter.Controllers
{
    [Area("Exporter")]
    public class PRNSController : BaseController
    {
        private IPRNService _prnService;

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

            return RedirectToAction("SentTo", "PRNS", new { area = Category, tonnesViewModel.Id });
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{materialId}")]
        public async Task<IActionResult> CreatePrn(int? materialId)
        {
            if (materialId == null)
                return NotFound();

            var prnId = await _prnService.CreatePrnRecord(materialId.Value, Category);

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

            return RedirectToAction("WhatToDo", "PRNS", new { area = Category.ToString(), id = checkYourAnswersViewModel.Id });
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

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Handle redirection to CheckYourAnswers if this is where we originally came from
            if (context.HttpContext.Request.Query.ContainsKey(Constants.Strings.QueryStrings.ReturnToAnswers) &&
                context.HttpContext.Request.Query[Constants.Strings.QueryStrings.ReturnToAnswers] == Constants.Strings.QueryStrings.ReturnToAnswersYes &&
                context.Result is RedirectToActionResult)
            {
                var id = (context.Result as RedirectToActionResult).RouteValues["Id"].ToString();
                context.Result = RedirectToAction("CheckYourAnswers", "PRNS", new { area = Category.ToString(), id });
            }

            base.OnActionExecuted(context);
        }
    }
}