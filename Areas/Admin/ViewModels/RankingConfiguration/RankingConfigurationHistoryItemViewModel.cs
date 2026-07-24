namespace EduNexus.Areas.Admin.ViewModels.RankingConfiguration
{
    public class RankingConfigurationHistoryItemViewModel
    {
        public long RankingConfigurationId { get; set; }

        public decimal AssignmentPercentage { get; set; }

        public decimal QuizPercentage { get; set; }

        public decimal LessonProgressPercentage { get; set; }

        public bool IsActive { get; set; }

        public string UpdatedByName { get; set; } =
            string.Empty;

        public DateTime UpdatedAt { get; set; }

        public decimal TotalPercentage =>
            AssignmentPercentage
            + QuizPercentage
            + LessonProgressPercentage;
    }
}