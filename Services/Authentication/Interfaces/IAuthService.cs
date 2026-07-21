using EduNexus.ViewModels.Authentication;

namespace EduNexus.Services.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> ValidateLoginAsync(
            string username,
            string password);
    }
}