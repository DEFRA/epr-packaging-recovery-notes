﻿using AutoMapper;
using EPRN.Common.Constants;
using EPRN.Common.Enums;
using EPRN.Portal.Configuration;
using EPRN.Portal.Helpers.Filters;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Models;
using EPRN.Portal.Profiles;
using EPRN.Portal.RESTServices;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.Services.HomeServices;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Authentication;
using Strings = EPRN.Common.Constants.Strings;

namespace EPRN.Portal.Helpers.Extensions
{
    public static class DependencyHelper
    {
        /// <summary>
        /// Extension method to add services for DI during startup
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
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
            services
                .Configure<ServicesConfiguration>(configuration.GetSection(ServicesConfiguration.SectionName))
                .Configure<AppConfigSettings>(configuration.GetSection(AppConfigSettings.SectionName));

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
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(x => {
                    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                    var factory = x.GetRequiredService<IUrlHelperFactory>();
                    return factory.GetUrlHelper(actionContext);
                })
                .AddTransient<ICultureHelper, CultureHelper>()
                .AddTransient(typeof(ILocalizationHelper<>), typeof(LocalizationHelper<>))
                .AddSingleton<IQueryStringHelper, QueryStringHelper>()
                .AddTransient<IWasteService, WasteService>()
                .AddTransient<IUserBasedService, ExporterAndReprocessorHomeService>()
                .AddTransient<IUserBasedService, UserBasedExporterService>()
                .AddTransient<IUserBasedService, UserBasedReprocessorService>()
                .AddTransient<IHomeServiceFactory, HomeServiceFactory>()
                .AddSingleton<IUserRoleService, UserRoleService>() // must be available through lifetime of the system
                .AddSingleton<BackButtonViewModel>()
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddTransient<IHttpWasteService>(s =>
                    // create a new http service using the configuration for restful services and a http client factory
                    new HttpWasteService(
                        s.GetRequiredService<IHttpContextAccessor>(),
                        s.GetRequiredService<IHttpClientFactory>(),
                        s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.Waste.Url,
                        Strings.ApiEndPoints.Waste)
                )
                .AddTransient<IHttpJourneyService>(s =>
                    // create a new http service using the configuration for restful services and a http client factory
                    new HttpJourneyService(
                        s.GetRequiredService<IHttpContextAccessor>(),
                        s.GetRequiredService<IHttpClientFactory>(),
                        s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.Waste.Url,
                        Strings.ApiEndPoints.Journey)
                )
                // Sometimes we want a PRN service that has a HTTPPRNService that is non specific
                // to the category so that we can view, search and create PRNs
                .AddTransient<IPRNService, PRNService>()
                .AddTransient<IHttpPrnsService>(s =>
                     new HttpPrnsService(
                         s.GetRequiredService<IHttpContextAccessor>(),
                         s.GetRequiredService<IHttpClientFactory>(),
                         s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.PRN.Url,
                         Strings.ApiEndPoints.PRN)
                )
                // We need a PRN service that has a HTTPPRNService relevant to the area it's in
                // so we can cross reference ID and Category to ensure we have the right PRN
                .AddTransient(s =>
                    new Func<Category, IPRNService>((category) =>
                    {
                        var httpPrnService = new HttpPrnsService(
                            s.GetRequiredService<IHttpContextAccessor>(),
                            s.GetRequiredService<IHttpClientFactory>(),
                            category,
                            s.GetRequiredService<IOptions<ServicesConfiguration>>().Value.PRN.Url,
                            Strings.ApiEndPoints.PRN);
                        return new PRNService(
                            s.GetRequiredService<IMapper>(),
                            httpPrnService);
                    })
                )
                .AddScoped<WasteTypeActionFilter>()
                .AddScoped<WasteCommonViewModel>(); // this needs to be available throughout the whole request, therefore needs to be scoped

            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
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