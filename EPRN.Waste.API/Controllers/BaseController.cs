using EPRN.Waste.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.Waste.API.Controllers
{
    public abstract class BaseController : Controller
    {
        private const string journeyIdName = "journeyId";
        protected readonly IJourneyService _journeyService;

        public BaseController(
            IJourneyService journeyService)
        {
            _journeyService = journeyService ?? throw new ArgumentNullException(nameof(journeyService));
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Method == HttpMethod.Get.Method)
            {
                var param = context.ActionDescriptor.Parameters;

                if (context.ActionDescriptor.Parameters.Any(p => p.Name == journeyIdName) &&
                    context.ActionArguments.ContainsKey(journeyIdName))
                {
                    int journeyId = Convert.ToInt32(context.ActionArguments[journeyIdName]);

                    if (!await _journeyService.JourneyExists(journeyId))
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