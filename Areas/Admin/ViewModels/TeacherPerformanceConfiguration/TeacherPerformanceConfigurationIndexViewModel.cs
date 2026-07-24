namespace EduNexus.Areas.Admin.ViewModels
    .TeacherPerformanceConfiguration
{
    public class TeacherPerformanceConfigurationIndexViewModel
    {
        public TeacherPerformanceConfigurationFormViewModel Form
        { get; set; } = new();

        public bool HasActiveConfiguration { get; set; }

        public long? ActiveConfigurationId { get; set; }

        public decimal? CurrentFeedbackPercentage { get; set; }

        public decimal? CurrentCompletionPercentage { get; set; }

        public decimal? CurrentStudentPerformancePercentage
        { get; set; }

        public int? CurrentMinimumEvaluationCount { get; set; }

        public string? CurrentUpdatedByName { get; set; }

        public DateTime? CurrentUpdatedAt { get; set; }

        public List<
            TeacherPerformanceConfigurationHistoryItemViewModel>
            History
        { get; set; } = new();
    }
}