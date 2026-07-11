namespace EduNexus.ViewModels.Student
{
    public class StudentAssignmentListViewModel
    {
        public long AssignmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string AssignmentType { get; set; } = string.Empty;
        public decimal? TotalScore { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssignmentStatus { get; set; } = string.Empty;   // Not Open / Open / Closed
        public string SubmissionStatus { get; set; } = string.Empty;   // Not Submitted / Draft / Submitted
        public bool IsSubmitted { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClosed { get; set; }
    }
}
