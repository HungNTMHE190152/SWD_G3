using EduNexus.Constants;
using EduNexus.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Data.SeedData
{
    public static class DbSeeder
    {
        public static async Task SeedDemoAccountsAsync(
            IServiceProvider serviceProvider)
        {
            using IServiceScope scope =
                serviceProvider.CreateScope();

            EduNexusContext context =
                scope.ServiceProvider
                    .GetRequiredService<EduNexusContext>();

            IPasswordHasher<Account> passwordHasher =
                scope.ServiceProvider
                    .GetRequiredService<IPasswordHasher<Account>>();

            await CreateAccountIfNotExistsAsync(
                context,
                passwordHasher,
                username: "admin",
                password: "Admin@123",
                fullName: "System Administrator",
                email: "admin@edunexus.local",
                roleName: RoleNames.Admin);

            await CreateAccountIfNotExistsAsync(
                context,
                passwordHasher,
                username: "sme",
                password: "Sme@123",
                fullName: "Demo SME",
                email: "sme@edunexus.local",
                roleName: RoleNames.Sme);

            await CreateAccountIfNotExistsAsync(
                context,
                passwordHasher,
                username: "teacher",
                password: "Teacher@123",
                fullName: "Demo Teacher",
                email: "teacher@edunexus.local",
                roleName: RoleNames.Teacher);

            await CreateAccountIfNotExistsAsync(
                context,
                passwordHasher,
                username: "student",
                password: "Student@123",
                fullName: "Demo Student",
                email: "student@edunexus.local",
                roleName: RoleNames.Student);
        }

        private static async Task CreateAccountIfNotExistsAsync(
            EduNexusContext context,
            IPasswordHasher<Account> passwordHasher,
            string username,
            string password,
            string fullName,
            string email,
            string roleName)
        {
            bool accountExists = await context.Accounts
                .AnyAsync(a => a.Username == username);

            if (accountExists)
            {
                return;
            }

            Role? role = await context.Roles
                .FirstOrDefaultAsync(r =>
                    r.RoleName == roleName);

            if (role == null)
            {
                throw new InvalidOperationException(
                    $"Role '{roleName}' was not found.");
            }

            User user = new User
            {
                FullName = fullName,
                Email = email,
                Status = UserStatuses.Active,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            Account account = new Account
            {
                UserId = user.UserId,
                RoleId = role.RoleId,
                Username = username,
                PasswordHash = string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            account.PasswordHash =
                passwordHasher.HashPassword(account, password);

            context.Accounts.Add(account);
            await context.SaveChangesAsync();
        }
    }
}