namespace EduNexus.ServiceRegistrations;

public static class LessonServiceRegistration
{
    public static IServiceCollection AddLessonServices(this IServiceCollection services)
    {
        // Person 2 will register Course/Lesson/Progress services here.
        return services;
    }
}