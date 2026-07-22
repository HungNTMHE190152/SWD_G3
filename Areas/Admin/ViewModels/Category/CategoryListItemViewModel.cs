namespace EduNexus.Areas.Admin.ViewModels.Category
{
    public class CategoryListItemViewModel
    {
        public long CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int CourseCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}