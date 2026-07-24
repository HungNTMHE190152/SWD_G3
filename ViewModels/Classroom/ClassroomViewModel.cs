using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Classroom;

public class ClassroomViewModel
{
    public long ClassroomId { get; set; }

    [Required]
    public long CourseId { get; set; }

    [Required]
    [StringLength(100)]
    public string ClassName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string EnrollmentCode { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string Status { get; set; } = "OPEN";

    public List<SelectListItem> Courses { get; set; } = new();
}