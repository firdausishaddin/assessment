using System.Collections.Generic;
using assessment.Models;
using Microsoft.AspNetCore.Identity;

namespace assessment.Data
{
    public static class UserSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher<UserDto>();

                var user = new UserDto
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Username = "admin",
                    Email = "admin@test.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                user.Password = hasher.HashPassword(user, "Test@123");

                context.Users.Add(user);
                context.SaveChanges();

                Console.WriteLine("✅ User seeded.");
            }
        }
    }
}