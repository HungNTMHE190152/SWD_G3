using EduNexus.ViewModels.Quizzes;

namespace EduNexus.Services.Interfaces;

public interface IQuizAttemptService
{
    Task<NewQuizViewModel> GetAvailableQuizzesAsync(long studentId);

    Task<long> StartAttemptAsync(long quizId, long studentId);

    Task<QuizTakingViewModel?> GetTakingAsync(long attemptId, long studentId);

    Task<long> SubmitAsync(QuizSubmitViewModel model, long studentId);

    Task<QuizResultViewModel?> GetResultAsync(long attemptId, long studentId);

    Task<QuizHistoryViewModel> GetHistoryAsync(long studentId);

    Task<QuizReviewViewModel?> GetReviewAsync(long attemptId, long studentId);
}