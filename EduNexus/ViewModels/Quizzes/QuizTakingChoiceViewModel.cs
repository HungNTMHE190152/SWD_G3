namespace EduNexus.ViewModels.Quizzes;

public class QuizTakingChoiceViewModel
{
    public long ChoiceId { get; set; }

    public string ChoiceContent { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}