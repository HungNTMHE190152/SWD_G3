using System.Collections.Generic;
using System.Threading.Tasks;
using EduNexus.ViewModels.Questions;

namespace EduNexus.Services.Interfaces
{
    public interface IQuestionBankService
    {
        Task<List<QuestionBankListViewModel>> GetBanksByCourseAsync(long courseId);
        Task<QuestionBankFormViewModel?> GetBankByIdAsync(long id);
        Task<long> CreateBankAsync(QuestionBankFormViewModel model, long smeId);
        Task<bool> UpdateBankAsync(QuestionBankFormViewModel model);
        Task<bool> DeleteBankAsync(long id);
    }
}
