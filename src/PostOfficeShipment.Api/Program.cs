using Microsoft.EntityFrameworkCore;
using PostOfficeShipment.Api.ExceptionHandling;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Application.Services;
using PostOfficeShipment.Infrastructure.Data;
using PostOfficeShipment.Infrastructure.Repositories;

namespace PostOfficeShipment.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
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

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                .GetRequiredService<ShipmentDbContext>();

                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseHttpsRedirection();
            }

            app.UseExceptionHandler();

            app.UseCors("Frontend");
            app.MapControllers();

            app.Run();
        }
    }
}
