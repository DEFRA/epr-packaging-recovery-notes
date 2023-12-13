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
    public class WasteTypeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.TryGetValue("type", out var typeValue))
            {
                if (Enum.TryParse<WasteType>(typeValue.ToString(), ignoreCase: true, out WasteType wasteType))
                {
                    // Successfully parsed, set it as a parameter in the action method
                    context.ActionArguments["type"] = wasteType;

                    // Set it as a private member variable in the controller
                    var controller = context.Controller as WasteController;
                    controller._wasteType = wasteType;

                    // Continue with the action execution
                    base.OnActionExecuting(context);
                }
                else
                {
                    // Parsing failed, short-circuit the request and return BadRequest
                    context.Result = new BadRequestObjectResult("Invalid WasteType value");
                }
            }
        }
    }
}
