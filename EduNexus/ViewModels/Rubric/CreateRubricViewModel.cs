using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Rubric
{
    public class CreateRubricViewModel
    {
        [Required]
        public long AssignmentId { get; set; }

        [Required(ErrorMessage = "Tên Rubric là bắt buộc")]
        [StringLength(200)]
        [Display(Name = "Tên Rubric")]
        public string RubricName { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Tổng trọng số là bắt buộc")]
        [Range(0.01, 1000, ErrorMessage = "Tổng trọng số phải lớn hơn 0")]
        [Display(Name = "Tổng trọng số (%)")]
        public decimal TotalWeight { get; set; }
    }
}
