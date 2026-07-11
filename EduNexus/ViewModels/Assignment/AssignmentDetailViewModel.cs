namespace EduNexus.ViewModels.Assignment;

public class AssignmentDetailViewModel
{
    public long AssignmentId { get; set; }

    public string Title { get; set; } = "";

    public string? Description { get; set; }

    public string CourseName { get; set; } = "";

    public string ModuleName { get; set; } = "";

    public string AssignmentType { get; set; } = "";

    public decimal? TotalScore { get; set; }

    public DateTime? OpenDate { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? AllowLateSubmission { get; set; }

    public bool? IsAIGenerated { get; set; }

    public string? Status { get; set; }

    public string CreatedBy { get; set; } = "";
}