using EPRN.Portal.Middleware;

namespace EPRN.Portal.Helpers.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UsePrnMiddleware(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<CultureMiddleware>()
                .UseMiddleware<UserRoleMiddleware>();

        }
    }
}
