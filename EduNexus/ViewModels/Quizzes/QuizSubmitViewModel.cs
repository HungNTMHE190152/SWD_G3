namespace EduNexus.ViewModels.Quizzes;

public class QuizSubmitViewModel
{
    public long AttemptId { get; set; }

    public List<QuizSubmitAnswerViewModel> Answers { get; set; } = new();
}

public class QuizSubmitAnswerViewModel
{
    public long QuestionId { get; set; }

    public long? ChoiceId { get; set; }
}