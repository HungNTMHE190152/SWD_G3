namespace EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport
{
    public class TeacherPerformanceClassroomViewModel
    {
        public long ClassroomId { get; set; }

        public long CourseId { get; set; }

        public string ClassName { get; set; } =
            string.Empty;

        public string CourseCode { get; set; } =
            string.Empty;

        public string CourseTitle { get; set; } =
            string.Empty;

        public string Status { get; set; } =
            string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int EnrollmentCount { get; set; }

        public int CompletedEnrollmentCount { get; set; }

        public int EvaluationCount { get; set; }

        public decimal FeedbackScore { get; set; }

        public decimal CompletionScore { get; set; }

        public decimal AssignmentScore { get; set; }

        public decimal QuizScore { get; set; }

        public decimal LessonProgressScore { get; set; }

        public decimal StudentPerformanceScore { get; set; }

        public decimal OverallScore { get; set; }

        public bool IsCompletionEligible =>
            Status == "OPEN"
            || Status == "IN_PROGRESS"
            || Status == "COMPLETED";
    }
}