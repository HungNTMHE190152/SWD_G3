namespace EduNexus.ViewModels.Grading
{
    public class GradeSubmissionViewModel
    {
        public long SubmissionId { get; set; }
        public long AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? SubmissionText { get; set; }

        public long RubricId { get; set; }
        public string RubricName { get; set; } = string.Empty;

        public decimal TotalScore { get; set; }

        public List<RubricCriterionGradeViewModel> Criteria { get; set; } = new();
        
    }

    public class RubricCriterionGradeViewModel
    {
        public long CriterionId { get; set; }
        public string CriterionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }
        public decimal Score { get; set; }
        public string? Comment { get; set; }
    }
}