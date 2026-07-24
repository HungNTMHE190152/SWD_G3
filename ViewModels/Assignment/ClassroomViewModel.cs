using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Assignment;

public class ClassroomViewModel
{
    public long ClassroomId { get; set; }

    [Required]
    public long CourseId { get; set; }

    // Thêm dòng này
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string ClassName { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string EnrollmentCode { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string Status { get; set; } = "OPEN";

    public List<CourseOptionViewModel> Courses { get; set; } = new();
}