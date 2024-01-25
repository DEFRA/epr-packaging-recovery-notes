using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.Portal.Helpers.Filters
{
    public class WasteTypeActionFilter : IActionFilter
    {
        private IWasteService _wasteService;

        public WasteTypeActionFilter(
            IWasteService wasteService)
        {
            _wasteService = wasteService ?? throw new ArgumentNullException(nameof(wasteService));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
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