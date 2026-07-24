using EduNexus.Services.Assignment.Implementations;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.Services.SME.Interfaces;
using EduNexus.Services.SME.Implementations;

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
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<ILessonResourceService, LessonResourceService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IQuestionBankService, QuestionBankService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IChoiceService, ChoiceService>();
            services.AddScoped<IQuizTemplateService, QuizTemplateService>();
            services.AddScoped<IQuizTemplateQuestionService,
                           QuizTemplateQuestionService>();
            return services;
        }
    }
}