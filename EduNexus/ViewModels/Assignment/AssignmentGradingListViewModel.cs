namespace EduNexus.ViewModels.Assignment
{
    public class AssignmentGradingListViewModel
    {
        public long AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public int SubmissionCount { get; set; }
    }
}
