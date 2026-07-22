using EduNexus.Services.Administration.Implementations;
using EduNexus.Services.Administration.Interfaces;

namespace EduNexus.Extensions.ServiceRegistrations
{
    public static class AdminServiceRegistration
    {
        public static IServiceCollection AddAdminServices(
            this IServiceCollection services)
        {
            services.AddScoped<
                IAdminDashboardService,
                AdminDashboardService>();

            services.AddScoped<
                IUserManagementService,
                UserManagementService>();

            services.AddScoped<
    ICategoryService,
    CategoryService>();

            return services;
        }
    }
}