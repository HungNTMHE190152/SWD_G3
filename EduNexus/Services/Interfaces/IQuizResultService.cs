using System.Threading.Tasks;
using EduNexus.ViewModels.Quizzes;

namespace EduNexus.Services.Interfaces
{
    public interface IQuizResultService
    {
        Task<QuizHistoryViewModel> GetStudentQuizHistoryAsync(long quizId, long studentId);
        Task<QuizResultViewModel?> GetAttemptResultAsync(long attemptId, long studentId);
        Task<QuizReviewViewModel?> GetAttemptReviewAsync(long attemptId, long studentId);
    }
}
