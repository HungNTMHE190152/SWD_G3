using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Assignment
{
    public class AssignmentTemplateFormViewModel
    {
        public long AssignmentTemplateId { get; set; }

        [Required]
        public long CourseId { get; set; }

        public long? ModuleId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Instructions { get; set; }

        [Range(1, 1000)]
        public decimal TotalScore { get; set; }

        public bool IsActive { get; set; } = true;

        public List<CourseOptionViewModel> Courses { get; set; } = new();

        public List<ModuleOptionViewModel> Modules { get; set; } = new();
    }
}