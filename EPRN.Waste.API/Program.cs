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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IJourneyService, JourneyService>();
builder.Services.AddTransient<IWasteService, WasteService>();
builder.Services.AddTransient<IRepository, Repository>();

// add db context options
builder.Services.AddDbContext<EPRNContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("WasteConnectionString"))
);

builder.Services.Configure<AppConfigSettings>(builder.Configuration.GetSection(AppConfigSettings.SectionName));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new WasteManagementProfile());
    mc.AllowNullCollections = true;
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
