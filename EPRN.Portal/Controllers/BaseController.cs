using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.Portal.Controllers
{
    public abstract class BaseController : Controller
    {
        private const string PreviousUrlKey = "PreviousUrl";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var backButtonViewModel = context.HttpContext.RequestServices.GetRequiredService<BackButtonViewModel>();

            if (backButtonViewModel == null)
                return;

            backButtonViewModel.PreviousUrl = string.Empty;

            if (context.HttpContext.Request.Path == "/")
                return;

            if (TempData[PreviousUrlKey] == null)
                return;

            backButtonViewModel.PreviousUrl = TempData[PreviousUrlKey].ToString();

            // remove the key and value from the store just tp be on the safe side
            TempData.Remove(PreviousUrlKey);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            TempData[PreviousUrlKey] = context.HttpContext.Request.Path.HasValue ? context.HttpContext.Request.Path.Value : string.Empty;

            base.OnActionExecuted(context);
        }
    }
}
