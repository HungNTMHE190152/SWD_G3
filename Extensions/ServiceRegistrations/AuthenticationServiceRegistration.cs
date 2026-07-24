using EduNexus.Configuration;
using EduNexus.Models;
using EduNexus.Services.Authentication.Implementations;
using EduNexus.Services.Authentication.Interfaces;
using EduNexus.Services.Email.Implementations;
using EduNexus.Services.Email.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace EduNexus.Extensions.ServiceRegistrations
{
    public static class AuthenticationServiceRegistration
    {
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.Configure<MailSettings>(
                configuration.GetSection("MailSettings"));

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<
                IPasswordHasher<Account>,
                PasswordHasher<Account>>();

            return services;
        }
    }
}