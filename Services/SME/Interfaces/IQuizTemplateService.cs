using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface IQuizTemplateService
{
    Task<List<QuizTemplate>> GetAllAsync();

    Task<QuizTemplate?> GetByIdAsync(long id);

    Task<bool> CreateAsync(QuizTemplate quizTemplate);

    Task<bool> UpdateAsync(QuizTemplate quizTemplate);

    Task<bool> DeleteAsync(long id);

    Task<List<Course>> GetCoursesAsync();

    Task<List<Module>> GetModulesAsync(long courseId);

    Task<List<Question>> GetQuestionsAsync(long courseId);
}