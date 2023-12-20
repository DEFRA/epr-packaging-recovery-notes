using EPRN.Portal.Constants;
using EPRN.Portal.Helpers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.Portal.Controllers
{
    [TypeFilter(typeof(JourneyTypeFilterAttribute))]
    [Route("[controller]/{type:JourneyType}/[action]/{id?}")]
    public class PRNSController : Controller
    {
        public JourneyType JourneyType { get; set; }

        private IPRNService _prnService;

        public PRNSController(IPRNServiceFactory prnServiceFactory)
        {
            if (prnServiceFactory == null)
                throw new ArgumentNullException(nameof(prnServiceFactory));

            _prnService = prnServiceFactory.CreatePRNService(JourneyType);
        }

        public IActionResult Tonnes(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = _prnService.GetTonnesViewModel(id.Value);

            return View(viewModel);
        }

        public async Task<IActionResult> Tonnes(TonnesViewModel tonnesViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(tonnesViewModel);
            }

            await _prnService.SaveTonnes(tonnesViewModel);

            return RedirectToAction("Index", "Home");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var viewResult = context.Result as ViewResult;

            if (context.HttpContext.Request.Method == HttpMethod.Get.Method && viewResult != null)
            {
                var viewName = viewResult.ViewName;

                if (string.IsNullOrWhiteSpace(viewName))
                    viewName = context.ActionDescriptor.RouteValues["action"];

                viewResult.ViewName = $"{viewName}.Export";
            }

            base.OnActionExecuted(context);
        }
    }
}
