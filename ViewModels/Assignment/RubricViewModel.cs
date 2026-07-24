using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Assignment
{
    public class RubricViewModel
    {
        public long RubricId { get; set; }

        public long AssignmentTemplateId { get; set; }

        public string AssignmentTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public List<RubricCriterionViewModel> Criteria { get; set; } = new();
        public decimal TotalScore { get; set; }
    }
}