using AutoMapper;
using EPRN.Common.Data;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Middleware;
using EPRN.Waste.API.Profiles;
using EPRN.Waste.API.Repositories;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPRN.Waste.API;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddTransient<IJourneyService, JourneyService>();
        services.AddTransient<IWasteService, WasteService>();
        services.AddTransient<IRepository, Repository>();
        services.AddTransient<IQuarterlyDatesService, QuarterlyDatesService>();
        services.AddDbContext<EPRNContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("WasteConnectionString"))
        );
        
        services.Configure<AppConfigSettings>(_configuration.GetSection(AppConfigSettings.SectionName));
        
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new WasteManagementProfile());
            mc.AllowNullCollections = true;
        });
        
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseRouting();
        app.UseAuthorization();     
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}