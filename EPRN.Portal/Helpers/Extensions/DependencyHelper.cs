using AutoMapper;
using EPRN.Portal.Configuration;
using EPRN.Portal.Constants;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Models;
using EPRN.Portal.Profiles;
using EPRN.Portal.RESTServices;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.Services.HomeServices;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Authentication;

namespace EPRN.Portal.Helpers.Extensions
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
                    CultureConstants.English,
                    CultureConstants.Welsh
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
            services.Configure<ServicesConfiguration>(configuration.GetSection(ServicesConfiguration.SectionName));
            services.Configure<AppConfigSettings>(configuration.GetSection(AppConfigSettings.SectionName));

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
            services
                .AddTransient(typeof(ILocalizationHelper<>), typeof(LocalizationHelper<>))
                .AddSingleton<IQueryStringHelper, QueryStringHelper>()
                .AddTransient<IWasteService, WasteService>()
                .AddTransient<IHomeService, ExporterAndReprocessorHomeService>()
                .AddTransient<IHomeService, ExporterHomeService>()
                .AddTransient<IHomeService, ReprocessorHomeService>()
                .AddTransient<IHomeServiceFactory, HomeServiceFactory>()
                .AddTransient<IPRNService, PRNService>()
                .AddSingleton<IUserRoleService, UserRoleService>() // must be available through lifetime of the system
                .AddSingleton<BackButtonViewModel>()
                .AddTransient<IHttpWasteService>(s =>
                {
                    // create a new http service using the configuration for restful services and a http client factory
                    return new HttpWasteService(
                        s.GetRequiredService<IHttpContextAccessor>(),
                        s.GetRequiredService<IHttpClientFactory>(),
                        s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.Waste.Url,
                        Strings.ApiEndPoints.Waste);
                })
                .AddTransient<IHttpJourneyService>(s =>
                {
                    // create a new http service using the configuration for restful services and a http client factory
                    return new HttpJourneyService(
                        s.GetRequiredService<IHttpContextAccessor>(),
                        s.GetRequiredService<IHttpClientFactory>(),
                        s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.Waste.Url,
                        Strings.ApiEndPoints.Journey);
                })
                .AddTransient<IHttpPrnsService>(s =>
                {
                    return new HttpPrnsService(
                        s.GetRequiredService<IHttpContextAccessor>(),
                        s.GetRequiredService<IHttpClientFactory>(),
                        s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.PRN.Url,
                        Strings.ApiEndPoints.PRN);
                });

            // move where area views live so they exist in the same parent location as
            // as other views
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Views/Areas/{2}/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Areas/{2}/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WasteManagementProfile());
                mc.AddProfile(new PRNProfile());
                mc.AllowNullCollections = true;
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}