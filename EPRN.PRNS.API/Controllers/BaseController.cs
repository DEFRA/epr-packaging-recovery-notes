using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.PRNS.API.Controllers
{
    public class BaseController : Controller
    {
        private const string idParameter = "id";
        protected readonly IPrnService _prnService;

        public BaseController(
            IPrnService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        /// <summary>
        /// Ensures that for every request a check is made that the record exists
        /// in the db
        /// </summary>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Method == HttpMethod.Get.Method)
            {
                var param = context.ActionDescriptor.Parameters;

                if (context.ActionDescriptor.Parameters.Any(p => p.Name == idParameter) &&
                    context.ActionArguments.ContainsKey(idParameter))
                {
                    int id = Convert.ToInt32(context.ActionArguments[idParameter]);

                    if (!await _prnService.PrnRecordExists(id))
                    {
                        context.Result = NotFound();
                        return;
                    }
                }
            }

            await next();
        }
    }
}
