using Microsoft.EntityFrameworkCore;
using SWIFTCARGOAPI.Models;

namespace SWIFTCARGOAPI.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                // Check if there are any users at all
                if (await context.Users.AnyAsync())
                {
                    // If there are users, check if an admin exists
                    if (await context.Users.AnyAsync(u => u.Role == "Admin"))
                    {
                        return; // Admin already exists
                    }
                }

                var adminUsername = configuration["AdminUser:Username"] ?? "admin";
                var adminPassword = configuration["AdminUser:Password"] ?? "Admin@123";

                var adminUser = new User
                {
                    Username = adminUsername,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    Role = "Admin"
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}