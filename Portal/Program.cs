using Microsoft.AspNetCore.Localization;
using Portal.Constants;
using Portal.Helpers;
using Portal.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDependencies(builder.Configuration);

var supportedCultures = new[]
{
    Constants.CultureConstants.English,
    Constants.CultureConstants.Welsh
};

builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
builder.Services
    .Configure<RequestLocalizationOptions>(opts =>
    {
        opts.DefaultRequestCulture = new RequestCulture(Constants.CultureConstants.English);
        opts.SupportedCultures = supportedCultures;
        opts.SupportedUICultures = supportedCultures;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCultureMiddleware();
app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();