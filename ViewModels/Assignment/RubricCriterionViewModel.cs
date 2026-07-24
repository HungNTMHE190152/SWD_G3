using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Assignment
{
    public class RubricCriterionViewModel
    {
        public long RubricCriterionId { get; set; }

        [Required]
        [StringLength(200)]
        public string CriterionName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, 1000)]
        public decimal MaxScore { get; set; }

        public int DisplayOrder { get; set; }
    }
}