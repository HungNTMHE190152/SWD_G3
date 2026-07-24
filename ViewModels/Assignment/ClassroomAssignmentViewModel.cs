using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduNexus.ViewModels.Assignment;

public class ClassroomAssignmentViewModel
{
    public long ClassroomAssignmentId { get; set; }

    [Required]
    public long ClassroomId { get; set; }

    [Required]
    public long AssignmentTemplateId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime OpenDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public bool AllowLateSubmission { get; set; }

    public string Status { get; set; } = "Published";

    // Hiển thị trên Index
    public string ClassroomName { get; set; } = string.Empty;

    public string AssignmentTemplateName { get; set; } = string.Empty;

    // Dropdown
    public List<SelectListItem> Classrooms { get; set; } = new();

    public List<SelectListItem> Assignments { get; set; } = new();
}

//public List<SelectListItem> Classrooms { get; set; } = new();

//public List<SelectListItem> Assignments { get; set; } = new();
