namespace EduNexus.Areas.Admin.ViewModels
    .TeacherPerformanceConfiguration
{
    public class TeacherPerformanceConfigurationHistoryItemViewModel
    {
        public long TeacherPerformanceConfigurationId { get; set; }

        public decimal FeedbackPercentage { get; set; }

        public decimal CompletionPercentage { get; set; }

        public decimal StudentPerformancePercentage { get; set; }

        public int MinimumEvaluationCount { get; set; }

        public bool IsActive { get; set; }

        public string UpdatedByName { get; set; } =
            string.Empty;

        public DateTime UpdatedAt { get; set; }

        public decimal TotalPercentage =>
            FeedbackPercentage
            + CompletionPercentage
            + StudentPerformancePercentage;
    }
}