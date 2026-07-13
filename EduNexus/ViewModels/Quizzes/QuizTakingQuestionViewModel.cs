namespace EduNexus.ViewModels.Quizzes;

public class QuizTakingQuestionViewModel
{
    public long QuestionId { get; set; }

    public string QuestionContent { get; set; } = string.Empty;

    public string QuestionType { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public List<QuizTakingChoiceViewModel> Choices { get; set; } = new();
}