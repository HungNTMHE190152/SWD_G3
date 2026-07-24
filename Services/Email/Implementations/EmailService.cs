using EduNexus.Configuration;
using EduNexus.Services.Email.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EduNexus.Services.Email.Implementations;

public class EmailService : IEmailService
{
    private readonly MailSettings _settings;

    public EmailService(
        IOptions<MailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendOtpAsync(
        string email,
        string otp)
    {
        MimeMessage message = new();

        message.From.Add(
            new MailboxAddress(
                _settings.SenderName,
                _settings.SenderEmail));

        message.To.Add(
            MailboxAddress.Parse(email));

        message.Subject = "EduNexus - Password Reset OTP";

        BodyBuilder builder = new();

        builder.HtmlBody = $@"
            <h2>EduNexus</h2>

            <p>Your OTP code is:</p>

            <h1 style='color:#0d6efd'>
                {otp}
            </h1>

            <p>
                This OTP will expire in
                <b>5 minutes</b>.
            </p>

            <p>
                Please do not share this code.
            </p>";

        message.Body = builder.ToMessageBody();

        using SmtpClient client = new();

        await client.ConnectAsync(
            _settings.Host,
            _settings.Port,
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _settings.SenderEmail,
            _settings.Password);

        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }
}