using EduNexus.ViewModels.Authentication;

namespace EduNexus.Services.Authentication.Interfaces;

public interface IAuthService
{
    Task<LoginResult> ValidateLoginAsync(
        string username,
        string password);

    Task<RegisterResult> RegisterAsync(
        RegisterViewModel model);
    Task<bool> SendOtpAsync(string email);

    Task<bool> VerifyOtpAsync(string email, string otp);

    Task<bool> ResetPasswordAsync(
        string email,
        string otp,
        string newPassword);
}