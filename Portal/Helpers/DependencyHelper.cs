using AutoMapper;
using Microsoft.Extensions.Options;
using Portal.Models;
using Portal.Services;
using Portal.Services.Interfaces;

namespace PRN.Web.Helpers
{
    public static class DependencyHelper
    {
        /// <summary>
        /// Extension method to add services for DI during startup
        /// </summary>
        public static IServiceCollection AddDependencies(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var section = configuration.GetSection("Services");
            services.Configure<ServicesConfiguration>(section);

            services.AddSingleton<IWasteService>(s =>
            {
                var url = s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.Waste.Url;

                return new WasteService(
                    url,
                    s.GetRequiredService<IMapper>(),
                    s.GetRequiredService<IHttpClientFactory>());
            });

            return services;
        }
    }
}
