using Portal.Models;
using Portal.Services.Implementations;
using Portal.Services.Interfaces;

namespace PRN.Web.Helpers
{
    public static class DependencyHelper
    {
        /// <summary>
        /// Extension method to add services for DI during startup
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<ServicesConfiguration>(configuration.GetSection("Services"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IWasteService, WasteService>();

            return services;
        }
    }
}