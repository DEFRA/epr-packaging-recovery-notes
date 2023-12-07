using Microsoft.AspNetCore.Localization;

namespace EPRN.Portal.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string _cookieName = CookieRequestCultureProvider.DefaultCookieName;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var cultureName = GetCultureFromRequest(context.Request);

            if (cultureName != null)
            {
                var culture = new RequestCulture(cultureName);
                context.Response.Cookies.Append(_cookieName,
                    CookieRequestCultureProvider.MakeCookieValue(culture),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1)
                    });
            }

            await _next(context);
        }

        private string GetCultureFromRequest(HttpRequest request)
        {
            return request.Query["culture"].FirstOrDefault();
        }
    }
}
