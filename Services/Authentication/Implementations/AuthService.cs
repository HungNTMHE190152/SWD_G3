using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Authentication.Interfaces;
using EduNexus.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Authentication.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly EduNexusContext _context;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AuthService(
            EduNexusContext context,
            IPasswordHasher<Account> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResult> ValidateLoginAsync(
            string username,
            string password)
        {
            string normalizedUsername = username.Trim();

            Account? account = await _context.Accounts
                .Include(a => a.User)
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a =>
                    a.Username == normalizedUsername);

            if (account == null)
            {
                return LoginResult.Failure(
                    "Invalid username or password.");
            }

            if (!account.IsActive)
            {
                return LoginResult.Failure(
                    "This account has been locked or disabled.");
            }

            if (account.User.Status != UserStatuses.Active)
            {
                return LoginResult.Failure(
                    "This user is currently inactive.");
            }

            PasswordVerificationResult passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    account,
                    account.PasswordHash,
                    password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return LoginResult.Failure(
                    "Invalid username or password.");
            }

            if (passwordResult ==
                PasswordVerificationResult.SuccessRehashNeeded)
            {
                account.PasswordHash =
                    _passwordHasher.HashPassword(account, password);

                account.UpdatedAt = DateTime.UtcNow;
            }

            account.LastLoginAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return LoginResult.Success(
                account.AccountId,
                account.UserId,
                account.User.FullName,
                account.User.Email,
                account.Role.RoleName);
        }
    }
}