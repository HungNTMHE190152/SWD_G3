namespace EduNexus.ViewModels.Student
{
    public class StudentSubmissionDetailViewModel
    {
        public long SubmissionId { get; set; }
        public long AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string? AssignmentDescription { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;

        public string? SubmissionText { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string SubmissionStatus { get; set; } = string.Empty;
        public string GradingStatus { get; set; } = string.Empty;

        public List<SubmissionAttachmentViewModel> Attachments { get; set; } = new();
        public bool IsDraft { get; set; }
    }
    public class SubmissionAttachmentViewModel
    {
        public long AttachmentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public DateTime? UploadedAt { get; set; }
    }
}
