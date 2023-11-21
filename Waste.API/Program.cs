using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Waste.API.Helpers;
using WasteManagement.API.Data;
using WasteManagement.API.Middleware;
using WasteManagement.API.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new WasteManagementProfile());
    mc.AllowNullCollections = true;
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// add db context options
builder.Services.AddDbContext<WasteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WasteConnectionString"))
);

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
