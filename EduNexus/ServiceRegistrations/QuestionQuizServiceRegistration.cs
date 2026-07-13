using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;

namespace EduNexus.ServiceRegistrations;

public static class QuestionQuizServiceRegistration
{
    public static IServiceCollection AddQuestionQuizServices(this IServiceCollection services)
    {
        services.AddScoped<IQuestionBankService, QuestionBankService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IQuizAttemptService, QuizAttemptService>();

        return services;
    }
}