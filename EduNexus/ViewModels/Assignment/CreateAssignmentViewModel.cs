using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Assignment
{
    public class CreateAssignmentViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select a Module.")]
        public long ModuleId { get; set; }

        [Required(ErrorMessage = "Assignment Type is required.")]
        public string AssignmentType { get; set; } = string.Empty;

        [Range(0, 100)]
        public decimal? TotalScore { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? OpenDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }

        public bool AllowLateSubmission { get; set; }

        public bool IsAIGenerated { get; set; }

        public string Status { get; set; } = "Draft";
    }
}