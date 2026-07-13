using EduNexus.ViewModels.Questions;

namespace EduNexus.Services.Interfaces;

public interface IQuestionBankService
{
    Task<List<QuestionBankListItemViewModel>> GetBanksForSmeAsync(long smeId);
    Task<QuestionBankFormViewModel> BuildCreateFormAsync(long smeId);
    Task<long> CreateAsync(QuestionBankFormViewModel model, long smeId);
    Task<QuestionListViewModel?> GetQuestionListAsync(long bankId, long smeId);
}