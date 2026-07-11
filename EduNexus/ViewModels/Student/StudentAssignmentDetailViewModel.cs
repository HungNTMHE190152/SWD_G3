namespace EduNexus.ViewModels.Student
{
    public class StudentAssignmentDetailViewModel
    {
        public long AssignmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string AssignmentType { get; set; } = string.Empty;
        public decimal? TotalScore { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? AllowLateSubmission { get; set; }
        public string Status { get; set; } = string.Empty;
        public string SubmissionStatus { get; set; } = string.Empty;
        public bool IsOpen { get; set; }
        public bool IsClosed { get; set; }
        public bool IsSubmitted { get; set; }

        public List<QuestionItemViewModel> Questions { get; set; } = new();
    }
}
