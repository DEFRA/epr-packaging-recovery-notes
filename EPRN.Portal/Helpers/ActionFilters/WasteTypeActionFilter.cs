using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.Portal.Helpers.Filters
{
    /// <summary>
    /// This class is applied to the WasteController at the controller level
    /// It ensures for any Action that has an Id parameter it will retrieve the name
    /// of the waste material being used. This way, it avoids every action having
    /// to perform that task itself
    /// </summary>
    public class WasteTypeActionFilter : IActionFilter
    {
        private readonly IWasteService _wasteService;

        public WasteTypeActionFilter(
            IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // does nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // need to get the journey id from the route data
            var idValue = context.HttpContext.Request.RouteValues["id"];

            if (idValue == null ||
                !int.TryParse(idValue.ToString(), out int id))
                return;

            var wasteName = Task.Run(async () => await _wasteService.GetWasteType(id)).Result;

            var wasteCommonViewModel = context.HttpContext.RequestServices.GetService<WasteCommonViewModel>();
            wasteCommonViewModel.WasteName = wasteName;
        }
    }
}