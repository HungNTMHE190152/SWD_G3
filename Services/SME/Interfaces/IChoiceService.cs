using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface IChoiceService
{
    Task<List<Choice>> GetByQuestionAsync(long questionId);

    Task<Choice?> GetByIdAsync(long id);

    Task<bool> CreateAsync(Choice choice);

    Task<bool> UpdateAsync(Choice choice);

    Task<bool> DeleteAsync(long id);
}