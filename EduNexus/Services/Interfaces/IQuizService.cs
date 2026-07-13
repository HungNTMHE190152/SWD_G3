using EduNexus.ViewModels.Quizzes;

namespace EduNexus.Services.Interfaces;

public interface IQuizService
{
    Task<List<QuizListItemViewModel>> GetQuizzesForSmeAsync(long smeId);

    Task<QuizFormViewModel> BuildCreateFormAsync(long smeId);

    Task<long> CreateQuizAsync(QuizFormViewModel model, long smeId);

    Task<QuizQuestionSelectionViewModel?> BuildQuestionSelectionAsync(
        long quizId,
        long smeId,
        long? bankId);

    Task UpdateQuizQuestionsAsync(
        QuizQuestionSelectionViewModel model,
        long smeId);
}