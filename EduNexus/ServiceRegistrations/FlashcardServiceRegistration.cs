using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;

namespace EduNexus.ServiceRegistrations;

public static class FlashcardServiceRegistration
{
    public static IServiceCollection AddFlashcardServices(this IServiceCollection services)
    {
        services.AddScoped<IFlashcardService, FlashcardService>();

        return services;
    }
}