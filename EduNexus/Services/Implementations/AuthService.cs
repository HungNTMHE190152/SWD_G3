using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Auth;
using Microsoft.EntityFrameworkCore;


namespace EduNexus.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly EduNexusContext _context;

    public AuthService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<LoginResultViewModel?> ValidateLoginAsync(string username, string password)
    {
        var account = await _context.Accounts
            .Include(a => a.User)
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Username == username);

        if (account == null)
        {
            return null;
        }

        if (account.IsActive != true)
        {
            return null;
        }

        if (account.User.Status != true)
        {
            return null;
        }

        bool isPasswordValid = account.PasswordHash == password;

        if (!isPasswordValid)
        {
            return null;
        }

        return new LoginResultViewModel
        {
            UserId = account.UserId,
            AccountId = account.AccountId,
            FullName = account.User.FullName,
            Email = account.User.Email,
            RoleName = account.Role.RoleName,
            IsActive = account.IsActive ?? false,
            IsVerified = account.IsVerified ?? false
        };
    }

    public async Task UpdateLastLoginAsync(long accountId)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountId == accountId);

        if (account == null)
        {
            return;
        }

        account.LastLogin = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task RegisterStudentAsync(RegisterViewModel model)
    {
        bool usernameExists = await _context.Accounts
            .AnyAsync(a => a.Username == model.Username);

        if (usernameExists)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        bool emailExists = await _context.Users
            .AnyAsync(u => u.Email == model.Email);

        if (emailExists)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var studentRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == RoleNames.Student);

        if (studentRole == null)
        {
            throw new InvalidOperationException("Student role does not exist in database.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        var user = new EduNexus.Models.User
        {
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            Phone = model.Phone,
            Status = true,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var account = new Account
        {
            UserId = user.UserId,
            RoleId = studentRole.RoleId,
            Username = model.Username.Trim(),
            PasswordHash = model.Password,
            IsActive = true,
            IsVerified = true,
            CreatedAt = DateTime.Now
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}