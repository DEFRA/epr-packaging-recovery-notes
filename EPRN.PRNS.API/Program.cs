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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPrnService, PrnService>();
builder.Services.AddTransient<IRepository, Repository>();
// add db context options

// add db context options
builder.Services.AddDbContext<EPRNContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("WasteConnectionString"))
);

builder.Services.Configure<AppConfigSettings>(builder.Configuration.GetSection(AppConfigSettings.SectionName));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new PrnManagementProfile());
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