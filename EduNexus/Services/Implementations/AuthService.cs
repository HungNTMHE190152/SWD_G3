using EduNexus.Data;
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
}