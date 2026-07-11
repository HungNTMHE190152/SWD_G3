using System.Collections.Generic;
using System.Threading.Tasks;
using EduNexus.ViewModels.Quizzes;

namespace EduNexus.Services.Interfaces
{
    public interface IQuizService
    {
        Task<List<QuizListViewModel>> GetQuizzesByAssignmentAsync(long assignmentId);
        Task<QuizFormViewModel?> GetQuizByIdAsync(long quizId);
        Task<long> CreateQuizAsync(QuizFormViewModel model, long smeId);
        Task<bool> UpdateQuizAsync(QuizFormViewModel model);
        Task<bool> DeleteQuizAsync(long quizId);
        
        Task<QuizQuestionSelectionViewModel> GetQuizQuestionSelectionAsync(long quizId, long bankId);
        Task<bool> AddQuestionsToQuizAsync(long quizId, List<long> questionIds);
        Task<bool> RemoveQuestionFromQuizAsync(long quizId, long questionId);
    }
}
