using EduNexus.ViewModels.Auth;

namespace EduNexus.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResultViewModel?> ValidateLoginAsync(string username, string password);

    Task UpdateLastLoginAsync(long accountId);
}