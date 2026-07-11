using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;

namespace EduNexus.ServiceRegistrations
{
    public static class AssignmentServiceRegistration
    {
        public static IServiceCollection AddAssignmentServices(this IServiceCollection services)
        {
            services.AddScoped<IAssignmentService, AssignmentService>();

            return services;
        }
    }
}