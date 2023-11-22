﻿using Microsoft.AspNetCore.Localization;
using Portal;

namespace Portal.Helpers
{
    public static class CultureHelper
    {
        public static (string oppositeCultureValue, string oppositeCultureName) GetCultureInfo(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null. The operation requires a valid HttpContext.");
            }

            var requestCultureInfo = httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture?.Culture;
            var isEnglish = Constants.Constants.CultureConstants.English.Name == requestCultureInfo?.Name;
            var oppositeCultureValue = isEnglish ? Constants.Constants.CultureConstants.Welsh.Name : Constants.Constants.CultureConstants.English.Name;
            var oppositeCultureName = isEnglish ? "Welsh" : "English";

            return (oppositeCultureValue, oppositeCultureName);
        }
    }
}
