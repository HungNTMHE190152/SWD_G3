using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface IQuizTemplateQuestionService
{
    Task<List<QuizTemplateQuestion>> GetByQuizAsync(long quizTemplateId);

    Task<QuizTemplateQuestion?> GetByIdAsync(long id);

    Task<bool> CreateAsync(QuizTemplateQuestion question);

    Task<bool> DeleteAsync(long id);

    Task<List<Question>> GetAvailableQuestionsAsync(long quizTemplateId);
}