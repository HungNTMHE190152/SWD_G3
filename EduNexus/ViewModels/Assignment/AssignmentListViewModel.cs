namespace EduNexus.ViewModels.Assignment
{
    public class AssignmentListViewModel
    {
        public long AssignmentId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public string ModuleName { get; set; } = string.Empty;

        public string AssignmentType { get; set; } = string.Empty;

        public decimal? TotalScore { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string? Status { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}