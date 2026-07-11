namespace EduNexus.ViewModels.Student
{
    public class AISubmissionResultViewModel
    {
        public long SubmissionId { get; set; }
        public long AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;

        // AI Grading Fields
        public decimal AIScore { get; set; }
        public decimal? MaxScore { get; set; }
        public string AIFeedback { get; set; } = string.Empty;
        public DateTime? AIGradedAt { get; set; }
        public string Status { get; set; } = string.Empty;

        // Chi tiết theo tiêu chí rubric
        public List<AICriterionResultViewModel> CriterionResults { get; set; } = new();

        // Các trường bổ sung (dùng cho View)
        public string OverallFeedback => AIFeedback;
        public decimal TotalScore => AIScore;
    }

    public class AICriterionResultViewModel
    {
        public string CriterionName { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}