namespace EduNexus.ViewModels.Submission
{
    public class SubmissionListViewModel
    {
        public long SubmissionId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public decimal? TotalScore { get; set; }
        public bool IsPublished { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
