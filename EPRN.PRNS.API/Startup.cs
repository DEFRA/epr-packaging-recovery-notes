using AutoMapper;
using EPRN.Common.Data;
using EPRN.PRNS.API.Configuration;
using EPRN.PRNS.API.Middleware;
using EPRN.PRNS.API.Profiles;
using EPRN.PRNS.API.Repositories;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPRN.PRNS.API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        
        public Startup(IHostEnvironment env, IConfiguration config)
        {
            _config = config;
        }
    
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<IPrnService, PrnService>();
            services.AddTransient<IRepository, Repository>();
            services.AddDbContext<EPRNContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("WasteConnectionString"))
            );
            services.Configure<AppConfigSettings>(_config.GetSection(AppConfigSettings.SectionName));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PrnManagementProfile());
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
}