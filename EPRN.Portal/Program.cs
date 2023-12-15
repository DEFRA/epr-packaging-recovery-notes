using EPRN.Portal.Constants;
using EPRN.Portal.Helpers;
using EPRN.Portal.Helpers.Extensions;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddDependencies(builder.Configuration);

var supportedCultures = new[]
{
    CultureConstants.English,
    CultureConstants.Welsh
};

builder.Services.AddLocalization(opts => 
{ 
    opts.ResourcesPath = "Resources"; 
});
builder.Services
    .Configure<RequestLocalizationOptions>(opts =>
    {
        opts.DefaultRequestCulture = new RequestCulture(CultureConstants.English);
        opts.SupportedCultures = supportedCultures;
        opts.SupportedUICultures = supportedCultures;
    });

builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("WasteType", typeof(WasteTypeConstraint));
});

// Check if in development environment
if (builder.Environment.IsDevelopment())
{
    // Add services with Razor Runtime Compilation
    builder.Services
        .AddControllersWithViews()
        .AddRazorRuntimeCompilation();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // in development, remove browser caching for our pages
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseStaticFiles();
}


app.UsePrnMiddleware();
app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
app.Run();