using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Module;

public class ModuleViewModel
{
    public long ModuleId { get; set; }

    [Required]
    public long CourseId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public int DisplayOrder { get; set; }
}