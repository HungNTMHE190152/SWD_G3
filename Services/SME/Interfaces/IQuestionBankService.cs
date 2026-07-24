using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface IQuestionBankService
{
    Task<List<QuestionBank>> GetAllAsync();

    Task<QuestionBank?> GetByIdAsync(long id);

    Task<List<Course>> GetCoursesAsync();

    Task<bool> CreateAsync(QuestionBank bank);

    Task<bool> UpdateAsync(QuestionBank bank);

    Task<bool> DeleteAsync(long id);
}