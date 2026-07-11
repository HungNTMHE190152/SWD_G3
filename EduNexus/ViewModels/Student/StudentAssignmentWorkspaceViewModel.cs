namespace EduNexus.ViewModels.Student
{
    public class StudentAssignmentWorkspaceViewModel
    {
        public long SubmissionId { get; set; }
        public long AssignmentId { get; set; }
        public long StudentId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string AssignmentType { get; set; } = string.Empty;

        public DateTime? OpenDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool AllowLateSubmission { get; set; }

        public string? SubmissionText { get; set; }
        public DateTime? LastSaved { get; set; }
        public string Status { get; set; } = "DRAFT";

        // Dùng cho Quiz/Mixed
        public List<QuestionAnswerViewModel> Questions { get; set; } = new();
    }
}
