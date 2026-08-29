using Microsoft.EntityFrameworkCore;
using PostOfficeShipment.Infrastructure.Data;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Infrastructure.Repositories;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ShipmentDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ShipmentDb")));

builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IPostOfficeRepository, PostOfficeRepository>();

builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IPostOfficeService, PostOfficeService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
