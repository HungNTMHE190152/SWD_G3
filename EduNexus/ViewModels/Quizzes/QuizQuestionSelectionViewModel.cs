using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduNexus.ViewModels.Quizzes;

public class QuizQuestionSelectionViewModel
{
    public long QuizId { get; set; }

    public string QuizTitle { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public long? BankId { get; set; }

    public List<SelectListItem> QuestionBanks { get; set; } = new();

    public List<QuizQuestionItemViewModel> Questions { get; set; } = new();
}