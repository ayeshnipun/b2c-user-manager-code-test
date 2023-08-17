using B2CUserManager.Models;
using B2CUserManager.Routes;
using B2CUserManager.Services.Implementations;
using B2CUserManager.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IUserManager, UserManager>();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AzureB2CSettings>(configuration.GetSection("B2C"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUserRoutes();

app.Run();