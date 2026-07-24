namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewQuizViewModel
    {
        public long QuizTemplateId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DefaultDurationMinutes { get; set; }

        public decimal DefaultPassingScore { get; set; }

        public int DefaultMaxAttempts { get; set; }

        public bool IsActive { get; set; }

        public int QuestionCount { get; set; }
    }
}