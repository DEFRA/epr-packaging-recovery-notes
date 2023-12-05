using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Waste.API.Configuration;
using Waste.API.Repositories;
using Waste.API.Repositories.Interfaces;
using Waste.API.Services;
using Waste.API.Services.Interfaces;
using WasteManagement.API.Data;
using WasteManagement.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IWasteService, WasteService>();
builder.Services.AddTransient<IRepository, Repository>();

// add db context options
builder.Services.AddDbContext<WasteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WasteConnectionString"))
);

builder.Services.Configure<AppConfigSettings>(builder.Configuration.GetSection(AppConfigSettings.SectionName));

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
