using AutoMapper;
using Microsoft.Extensions.Options;
using Waste.API.Services;
using Waste.API.Services.Interfaces;

namespace Waste.API.Helpers
{
    public static class DependencyHelper
    {
        /// <summary>
        /// Extension method to add services for DI during startup
        /// </summary>
        public static IServiceCollection AddDependencies(
            this IServiceCollection services)
        {
            services.AddTransient<IWasteService, WasteService>();

            return services;
        }
    }
}
