using EduNexus.ViewModels.Questions;

namespace EduNexus.Services.Interfaces;

public interface IQuestionService
{
    Task<QuestionFormViewModel?> BuildCreateFormAsync(long bankId, long smeId);
    Task<long> CreateAsync(QuestionFormViewModel model, long smeId);
}