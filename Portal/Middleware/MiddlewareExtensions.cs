namespace Portal.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CultureMiddleware>();
        }
    }
}
