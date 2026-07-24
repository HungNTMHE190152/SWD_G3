using EduNexus.Services.Assignment.Implementations;
using EduNexus.Services.Assignment.Interfaces;

namespace EduNexus.Extensions.ServiceRegistrations
{
    public static class SmeServiceRegistration
    {
        public static IServiceCollection AddSmeServices(
            this IServiceCollection services)
        {
            services.AddScoped<
                IAssignmentTemplateService,
                AssignmentTemplateService>();

            services.AddScoped<
                IRubricService,
                RubricService>();

            services.AddScoped<
                IClassroomAssignmentService,
                ClassroomAssignmentService>();
            services.AddScoped<
                IAssignmentSubmissionService, 
                AssignmentSubmissionService>();
            return services;
        }
    }
}