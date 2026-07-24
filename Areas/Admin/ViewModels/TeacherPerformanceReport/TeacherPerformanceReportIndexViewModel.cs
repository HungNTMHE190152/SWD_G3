namespace EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport
{
    public class TeacherPerformanceReportIndexViewModel
    {
        public TeacherPerformanceReportFilterViewModel Filter
        { get; set; } = new();

        public List<TeacherPerformanceReportItemViewModel> Teachers
        { get; set; } = new();

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int TotalTeacherCount { get; set; }

        public int SufficientDataCount { get; set; }

        public int InsufficientDataCount { get; set; }

        public decimal AverageOverallScore { get; set; }

        public string? TopTeacherName { get; set; }

        public decimal? TopTeacherScore { get; set; }

        public bool HasActivePerformanceConfiguration { get; set; }

        public bool HasActiveRankingConfiguration { get; set; }

        public decimal FeedbackWeightPercentage { get; set; }

        public decimal CompletionWeightPercentage { get; set; }

        public decimal StudentPerformanceWeightPercentage { get; set; }

        public decimal AssignmentWeightPercentage { get; set; }

        public decimal QuizWeightPercentage { get; set; }

        public decimal LessonProgressWeightPercentage { get; set; }

        public int MinimumEvaluationCount { get; set; }

        public bool HasPreviousPage =>
            Filter.Page > 1;

        public bool HasNextPage =>
            Filter.Page < TotalPages;
    }
}