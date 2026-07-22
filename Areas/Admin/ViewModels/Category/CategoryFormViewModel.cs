using System.ComponentModel.DataAnnotations;

namespace EduNexus.Areas.Admin.ViewModels.Category
{
    public class CategoryFormViewModel
    {
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(
            150,
            ErrorMessage = "Category name cannot exceed 150 characters.")]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(
            1000,
            ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}