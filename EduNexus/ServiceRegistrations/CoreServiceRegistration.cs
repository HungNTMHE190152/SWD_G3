using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;

namespace EduNexus.ServiceRegistrations;

public static class CoreServiceRegistration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}