using EduNexus.Models;
using EduNexus.Services.Authentication.Implementations;
using EduNexus.Services.Authentication.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EduNexus.Extensions.ServiceRegistrations
{
    public static class AuthenticationServiceRegistration
    {
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<
                IPasswordHasher<Account>,
                PasswordHasher<Account>>();

            return services;
        }
    }
}