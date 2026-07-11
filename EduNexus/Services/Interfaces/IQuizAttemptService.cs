using System.Collections.Generic;
using System.Threading.Tasks;
using EduNexus.ViewModels.Quizzes;

namespace EduNexus.Services.Interfaces
{
    public interface IQuizAttemptService
    {
        Task<List<NewQuizViewModel>> GetAvailableQuizzesForStudentAsync(long studentId);
        Task<NewQuizViewModel?> GetQuizDetailsForStudentAsync(long quizId, long studentId);
        Task<QuizTakingViewModel> StartQuizAsync(long quizId, long studentId);
        Task<QuizTakingViewModel?> GetQuizTakingModelAsync(long attemptId, long studentId);
        Task<bool> SubmitQuizAsync(QuizSubmitViewModel model, long studentId);
    }
}
