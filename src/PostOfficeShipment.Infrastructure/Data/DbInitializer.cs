using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ShipmentDbContext context, IConfiguration configuration)
    {
        await context.Database.MigrateAsync();

        var username = configuration["Authentication:AdminUsername"];

        var password = configuration["Authentication:AdminPassword"];

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (await context.Users.AnyAsync(x => x.Username == username))
        {
            return;
        }

        var user = new User(username, "temporary");

        var hasher = new PasswordHasher<User>();

        var hash = hasher.HashPassword(user, password);

        user = new User(username, hash, "Admin");

        context.Users.Add(user);

        await context.SaveChangesAsync();
    }
}