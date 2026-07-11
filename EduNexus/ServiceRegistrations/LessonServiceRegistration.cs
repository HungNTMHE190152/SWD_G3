using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;

namespace EduNexus.ServiceRegistrations;

public static class LessonServiceRegistration
{
    public static IServiceCollection AddLessonServices(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IProgressService, ProgressService>();
        services.AddScoped<IResourceService, ResourceService>();
        services.AddScoped<ILessonTranscriptService, LessonTranscriptService>();
        services.AddScoped<IAiLessonService, AiLessonService>();
        return services;
    }
}