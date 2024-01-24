using EPRN.Common.Constants;
using EPRN.Portal.Helpers.Extensions;
using Microsoft.AspNetCore.Localization;

namespace EPRN.Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }
        
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                services
                    .AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
            }
            
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddDependencies(Configuration);

            var supportedCultures = new[]
            {
                    CultureConstants.English,
                    CultureConstants.Welsh
                };
            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });
            services
                .Configure<RequestLocalizationOptions>(opts =>
                {
                    opts.DefaultRequestCulture = new RequestCulture(CultureConstants.English);
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                        ctx.Context.Response.Headers["Pragma"] = "no-cache";
                        ctx.Context.Response.Headers["Expires"] = "0";
                    }
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseHsts();
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseStaticFiles();
            }
            
            app.UsePrnMiddleware();
            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}
