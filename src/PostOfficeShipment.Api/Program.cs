using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PostOfficeShipment.Api.ExceptionHandling;
using PostOfficeShipment.Application.Authentication;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Application.Services;
using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Infrastructure.Data;
using PostOfficeShipment.Infrastructure.Repositories;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;

namespace PostOfficeShipment.Api
{
    public static class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));

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

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT signing key is not configured.");
            var jwtIssuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
            var jwtAudience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT audience is not configured.");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAudience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(
                                new JsonStringEnumConverter());
                            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // Use the HTTP bearer scheme which is the recommended way to describe JWT in OpenAPI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string> { }
                });
            });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                    .WithOrigins("http://localhost:5173", "http://localhost:5174")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ShipmentDbContext>();

                //dbContext.Database.Migrate();
                await DbInitializer.InitializeAsync(dbContext, app.Configuration);
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
