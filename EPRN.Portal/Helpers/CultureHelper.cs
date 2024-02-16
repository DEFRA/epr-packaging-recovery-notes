using EPRN.Portal.Helpers.Interfaces;
using Microsoft.AspNetCore.Localization;
using CultureConstants = EPRN.Common.Constants.CultureConstants;

namespace EPRN.Portal.Helpers
{
    public class CultureHelper : ICultureHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CultureHelper(IHttpContextAccessor httpContextAccessor)
        {
                _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public string GetCultureInfo()
        {
            var requestCultureInfo = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture?.Culture;
            var isEnglish = CultureConstants.English.Name == requestCultureInfo?.Name;
            var oppositeCultureValue = isEnglish ? CultureConstants.Welsh.Name : CultureConstants.English.Name;

            return oppositeCultureValue;
        }

        public string ShortCultureCode()
        {
            var requestCultureInfo = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture?.Culture;
            return requestCultureInfo.TwoLetterISOLanguageName;
        }
    }
}