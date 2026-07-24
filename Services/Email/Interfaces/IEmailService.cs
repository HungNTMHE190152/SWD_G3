namespace EduNexus.Services.Email.Interfaces;

public interface IEmailService
{
    Task SendOtpAsync(
        string email,
        string otp);
}