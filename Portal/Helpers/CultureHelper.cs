using Microsoft.AspNetCore.Localization;
using Portal.Constants;

namespace Portal.Helpers
{
    public static class CultureHelper
    {
        public static string GetCultureInfo(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null. The operation requires a valid HttpContext.");
            }

            var requestCultureInfo = httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture?.Culture;
            var isEnglish = CultureConstants.English.Name == requestCultureInfo?.Name;
            var oppositeCultureValue = isEnglish ? CultureConstants.Welsh.Name : CultureConstants.English.Name;

            return oppositeCultureValue;
        }
    }
}