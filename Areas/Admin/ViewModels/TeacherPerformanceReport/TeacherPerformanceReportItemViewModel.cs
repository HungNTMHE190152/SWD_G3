namespace EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport
{
    public class TeacherPerformanceReportItemViewModel
    {
        public long TeacherId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public string UserStatus { get; set; } = string.Empty;

        public bool IsAccountActive { get; set; }

        public int TotalClassroomCount { get; set; }

        public int EligibleClassroomCount { get; set; }

        public int CompletedClassroomCount { get; set; }

        public int EvaluationCount { get; set; }

        public int MinimumEvaluationCount { get; set; }

        public decimal FeedbackScore { get; set; }

        public decimal CompletionScore { get; set; }

        public decimal AssignmentScore { get; set; }

        public decimal QuizScore { get; set; }

        public decimal LessonProgressScore { get; set; }

        public decimal StudentPerformanceScore { get; set; }

        public decimal OverallScore { get; set; }

        public bool HasSufficientEvaluationData =>
            EvaluationCount >= MinimumEvaluationCount;
    }
}