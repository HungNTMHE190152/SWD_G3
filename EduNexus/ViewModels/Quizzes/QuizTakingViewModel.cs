namespace EduNexus.ViewModels.Quizzes
{
    public class QuizTakingViewModel
    {
        public long AttemptId { get; set; }
        public long QuizId { get; set; }

        public string QuizTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public List<QuizTakingQuestionViewModel> Questions { get; set; } = new();

    }
}
