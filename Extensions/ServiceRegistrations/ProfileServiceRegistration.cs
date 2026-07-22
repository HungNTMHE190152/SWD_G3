using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;

namespace EduNexus.Extensions.ServiceRegistrations
{
    public static class ProfileServiceRegistration
    {
        public static IServiceCollection AddProfileServices(
            this IServiceCollection services)
        {
            services.AddScoped<
                IProfileService,
                ProfileService>();

            return services;
        }
    }
}