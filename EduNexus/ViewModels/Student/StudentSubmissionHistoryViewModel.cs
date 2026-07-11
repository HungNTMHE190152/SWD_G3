namespace EduNexus.ViewModels.Student
{
    public class StudentSubmissionHistoryViewModel
    {
        public long SubmissionId { get; set; }
        public long AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public DateTime? SubmittedAt { get; set; }
        public string SubmissionStatus { get; set; } = string.Empty;
        public string GradingStatus { get; set; } = string.Empty;
        public bool IsDraft { get; set; }
    }
}
