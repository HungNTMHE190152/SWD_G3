using EduNexus.Services.Assignment.Interfaces;
using EduNexus.Services.Assignment.Implementations;

namespace EduNexus.Extensions.ServiceRegistrations
{
    public static class TeacherServiceRegistration
    {
        public static IServiceCollection AddTeacherServices(
            this IServiceCollection services)
        {
            services.AddScoped<IClassroomService, ClassroomService>();
            services.AddScoped<IClassroomAssignmentService, ClassroomAssignmentService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IAssignmentSubmissionService, AssignmentSubmissionService>();
            services.AddScoped<ISubmissionGradeService, SubmissionGradeService>();
            return services;
        }
    }
}