using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface IQuestionService
{
    Task<List<Question>> GetByBankAsync(long bankId);

    Task<Question?> GetByIdAsync(long id);

    Task<bool> CreateAsync(Question question);

    Task<bool> UpdateAsync(Question question);

    Task<bool> DeleteAsync(long id);
}