using System.Collections.Generic;
using System.Threading.Tasks;
using EduNexus.ViewModels.Questions;

namespace EduNexus.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionListViewModel>> GetQuestionsByBankAsync(long bankId);
        Task<QuestionDetailViewModel?> GetQuestionDetailAsync(long questionId);
        Task<QuestionFormViewModel?> GetQuestionForEditAsync(long questionId);
        Task<long> CreateQuestionAsync(QuestionFormViewModel model, long smeId);
        Task<bool> UpdateQuestionAsync(QuestionFormViewModel model);
        Task<bool> DeleteQuestionAsync(long questionId);
    }
}
