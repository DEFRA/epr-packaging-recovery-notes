using EPRN.Portal.Constants;
using EPRN.Portal.Helpers;
using EPRN.Portal.Middleware;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");
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
app.MapControllers();
app.Run();