using AutoMapper;
using Microsoft.Extensions.Options;
using Portal.Models;
using Portal.Profiles;
using Portal.RESTServices;
using Portal.RESTServices.Interfaces;
using Portal.Services;
using Portal.Services.Interfaces;
using System.Security.Authentication;

namespace Portal.Helpers
{
    public static class DependencyHelper
    {
        /// <summary>
        /// Extension method to add services for DI during startup
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
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
                return new HttpWasteService(
                    s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.Waste.Url,
                    s.GetRequiredService<IHttpClientFactory>());
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WasteProfiles());
                mc.AllowNullCollections = true;
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}