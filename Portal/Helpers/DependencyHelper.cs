using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Portal.Helpers.Interfaces;
using Portal.Models;
using Portal.Profiles;
using Portal.RESTServices;
using Portal.RESTServices.Interfaces;
using Portal.Services;
using Portal.Services.Interfaces;
using System.Globalization;
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
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                                            {
                                                Constants.CultureConstants.English,
                                                Constants.CultureConstants.Welsh
                                            };

                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddRazorPages()
                .AddViewOptions(o =>
                {
                    o.HtmlHelperOptions.ClientValidationEnabled = configuration.GetValue<bool>("ClientValidationEnabled");
                });

            // Get the configuration for the services
            services.Configure<ServicesConfiguration>(configuration.GetSection(ServicesConfiguration.Name));

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

            services.AddTransient(typeof(LocalizationHelper<>));
            services.AddSingleton<IQueryStringHelper, QueryStringHelper>();
            services.AddTransient<IWasteService, WasteService>();
            services.AddTransient<IHttpWasteService>(s =>
            {
                // create a new http service using the configuration for restful services and a http client factory
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