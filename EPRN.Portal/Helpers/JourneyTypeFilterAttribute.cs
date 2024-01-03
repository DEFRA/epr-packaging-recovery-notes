using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EPRN.Portal.Controllers;
using EPRN.Portal.Constants;

namespace EPRN.Portal.Helpers
{
    /// <summary>
    /// This class ensures that the member variable _wasteType
    /// of the WasteController is populated with the correct value 
    /// from the route provided in the url
    /// </summary>
    public class JourneyTypeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.TryGetValue("type", out var typeValue))
            {
                if (Enum.TryParse<JourneyType>(typeValue.ToString(), ignoreCase: true, out JourneyType journeyType))
                {
                    // Successfully parsed, set it as a parameter in the action method
                    context.ActionArguments["journeyType"] = journeyType;

                    // Set it as a private member variable in the controller
                    var controller = context.Controller as JourneyController;
                    controller.JourneyType = journeyType;

                    // Continue with the action execution
                    base.OnActionExecuting(context);
                }
                else
                {
                    // Parsing failed, short-circuit the request and return BadRequest
                    context.Result = new BadRequestObjectResult("Invalid JourneyType value");
                }
            }
        }
    }
}
