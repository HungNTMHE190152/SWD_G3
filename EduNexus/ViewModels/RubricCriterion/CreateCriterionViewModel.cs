using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.RubricCriterion
{
    public class CreateCriterionViewModel
    {
        public long RubricId { get; set; }

        [Required(ErrorMessage = "Tên tiêu chí là bắt buộc")]
        [StringLength(200)]
        [Display(Name = "Tên tiêu chí")]
        public string CriterionName { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 100, ErrorMessage = "Weight phải lớn hơn 0")]
        [Display(Name = "Weight (%)")]
        public decimal Weight { get; set; }

        [Required]
        [Range(0.01, 100, ErrorMessage = "Max Score phải lớn hơn 0")]
        [Display(Name = "Điểm tối đa")]
        public decimal MaxScore { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }
    }
}




