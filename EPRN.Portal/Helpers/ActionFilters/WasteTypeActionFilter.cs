using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPRN.Portal.Helpers.Attributes
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

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            // need to get the journey id from the route data
            var idValue = context.HttpContext.Request.RouteValues["id"];

            if (idValue == null ||
                !int.TryParse(idValue.ToString(), out int id))
                return;

            var wasteName = await _wasteService.GetWasteType(id);

            var wasteCommonViewModel = context.HttpContext.RequestServices.GetService<WasteCommonViewModel>();
            wasteCommonViewModel.WasteName = wasteName;
        }
    }
}