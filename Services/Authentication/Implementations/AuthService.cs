using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Authentication.Interfaces;
using EduNexus.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EduNexus.Models;
using EduNexus.ViewModels.Authentication;
using Microsoft.EntityFrameworkCore;
using EduNexus.Services.Email.Interfaces;
using Microsoft.Extensions.Caching.Memory;
//nothing here
namespace EduNexus.Services.Authentication.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly EduNexusContext _context;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;
        public AuthService(
      EduNexusContext context,
      IPasswordHasher<Account> passwordHasher,
      IEmailService emailService,
      IMemoryCache memoryCache)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _memoryCache = memoryCache;
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
        public async Task<RegisterResult> RegisterAsync(RegisterViewModel model)
        {
            string username = model.Username.Trim();
            string email = model.Email.Trim();

            // Kiểm tra Username
            bool usernameExists = await _context.Accounts
                .AnyAsync(x => x.Username == username);

            if (usernameExists)
            {
                return RegisterResult.Failure(
                    "Username already exists.");
            }

            // Kiểm tra Email
            bool emailExists = await _context.Users
                .AnyAsync(x => x.Email == email);

            if (emailExists)
            {
                return RegisterResult.Failure(
                    "Email already exists.");
            }

            // Lấy Role Student
            Role? studentRole = await _context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == RoleNames.Student);

            if (studentRole == null)
            {
                return RegisterResult.Failure(
                    "Student role was not found.");
            }

            User user = new()
            {
                FullName = model.FullName,
                Email = email,
                PhoneNumber = model.PhoneNumber,
                Status = UserStatuses.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            Account account = new()
            {
                UserId = user.UserId,
                RoleId = studentRole.RoleId,
                Username = username,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            account.PasswordHash =
                _passwordHasher.HashPassword(
                    account,
                    model.Password);

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();

            return RegisterResult.Success();
        }
        private static string GenerateOtp()
        {
            Random random = new();

            return random.Next(100000, 999999).ToString();
        }
        public async Task<bool> SendOtpAsync(string email)
        {
            User? user = await _context.Users
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return false;

            string otp = GenerateOtp();

            _memoryCache.Set(
                $"OTP_{email}",
                otp,
                TimeSpan.FromMinutes(5));

            await _emailService.SendOtpAsync(email, otp);

            return true;
        }
        public Task<bool> VerifyOtpAsync(
    string email,
    string otp)
        {
            if (!_memoryCache.TryGetValue(
                $"OTP_{email}",
                out string? savedOtp))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(savedOtp == otp);
        }
        public async Task<bool> ResetPasswordAsync(
    string email,
    string otp,
    string newPassword)
        {
            bool valid = await VerifyOtpAsync(
                email,
                otp);

            if (!valid)
                return false;

            Account? account = await _context.Accounts
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Email == email);

            if (account == null)
                return false;

            account.PasswordHash =
                _passwordHasher.HashPassword(
                    account,
                    newPassword);

            account.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _memoryCache.Remove($"OTP_{email}");

            return true;
        }
    }
}