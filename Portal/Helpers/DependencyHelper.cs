using Microsoft.Extensions.Options;
using Portal.Models;
using Portal.RESTServices.Interfaces;
using Portal.RESTServices;
using Portal.Services.Implementations;
using Portal.Services.Interfaces;
using System.Security.Authentication;

namespace PRN.Web.Helpers
{
    public static class DependencyHelper
    {
        /// <summary>
        /// Extension method to add services for DI during startup
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Get the configuration for the services
            services.Configure<ServicesConfiguration>(configuration.GetSection("Services"));

            services.AddHttpContextAccessor();
            services
                .AddHttpClient("HttpClient")
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler()
                    {
                        SslProtocols = SslProtocols.Tls12
                    };
                });
            services.AddTransient<IWasteService, WasteService>();
            services.AddTransient<IHttpWasteService>(s =>
            {
                // create a new http service using the configuration for restful services and a http client factory
                return new HttpWasteService(
                    s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.Waste.Url,
                    s.GetRequiredService<IHttpClientFactory>());
            });

            return services;
        }
    }
}