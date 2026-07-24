using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Course;

public class CourseViewModel
{
    public long CourseId { get; set; }

    [Required]
    public long CategoryId { get; set; }

    [Required]
    public string CourseCode { get; set; } = "";

    [Required]
    public string Title { get; set; } = "";

    public string? Description { get; set; }

    public string? ThumbnailUrl { get; set; }
}