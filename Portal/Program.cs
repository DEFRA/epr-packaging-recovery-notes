using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Portal.Profiles;
using PRN.Web.Constants;
using PRN.Web.Helpers;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDependencies(builder.Configuration);

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new WasteProfiles());
    mc.AllowNullCollections = true;
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

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

builder.Services
    .AddHttpClient("HttpClient")
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler()
        {
            SslProtocols = SslProtocols.Tls12
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Waste",
        pattern: "Waste/{action=Index}/{id}");
    endpoints.MapDefaultControllerRoute();
});

app.Run();
