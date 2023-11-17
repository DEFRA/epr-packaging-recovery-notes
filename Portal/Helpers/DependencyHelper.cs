using AutoMapper;
using Portal.Services;
using Portal.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace PRN.Web.Helpers
{
    public static class DependencyHelper
    {
        /// <summary>
        /// Extension method to add services for DI during startup
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IWasteService, WasteService>();
            services.AddSingleton<IMapper, Mapper>();

            return services;
        }
    }
}
