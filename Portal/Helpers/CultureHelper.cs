using PRN.Web.Constants;

namespace Portal.Helpers
{
    public static class CultureHelper
    {
        public static (string oppositeCultureValue, string oppositeCultureName) GetCultureInfo(HttpContext context)
        {
            var requestCultureInfo = context.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>()?.RequestCulture.Culture;
            var isEnglish = Constants.CultureConstants.English.Name == requestCultureInfo?.Name;
            var oppositeCultureValue = isEnglish ? Constants.CultureConstants.Welsh.Name : Constants.CultureConstants.English.Name;
            var oppositeCultureName = isEnglish ? "Welsh" : "English";

            return (oppositeCultureValue, oppositeCultureName);
        }
    }
}
