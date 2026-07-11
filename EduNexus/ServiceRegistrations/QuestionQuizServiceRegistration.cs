namespace EduNexus.ServiceRegistrations;

public static class QuestionQuizServiceRegistration
{
    public static IServiceCollection AddQuestionQuizServices(this IServiceCollection services)
    {
        // Person 5 will register Question/Quiz services here.
        services.AddScoped<EduNexus.Services.Interfaces.IQuestionBankService, EduNexus.Services.Implementations.QuestionBankService>();
        services.AddScoped<EduNexus.Services.Interfaces.IQuestionService, EduNexus.Services.Implementations.QuestionService>();
        services.AddScoped<EduNexus.Services.Interfaces.IChoiceService, EduNexus.Services.Implementations.ChoiceService>();
        services.AddScoped<EduNexus.Services.Interfaces.IQuizService, EduNexus.Services.Implementations.QuizService>();
        services.AddScoped<EduNexus.Services.Interfaces.IQuizAttemptService, EduNexus.Services.Implementations.QuizAttemptService>();
        services.AddScoped<EduNexus.Services.Interfaces.IQuizResultService, EduNexus.Services.Implementations.QuizResultService>();
        
        return services;
    }
}