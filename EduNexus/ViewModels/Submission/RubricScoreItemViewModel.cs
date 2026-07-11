namespace EduNexus.ViewModels.Submission
{
    public class RubricScoreItemViewModel
    {
        public long RubricScoreId { get; set; }
        public long RubricCriterionId { get; set; }
        public string CriterionName { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }
        public decimal Score { get; set; }
        public string? Feedback { get; set; }
    }
}
