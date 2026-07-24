namespace EduNexus.Areas.Admin.ViewModels.Category
{
    public class CategoryFilterViewModel
    {
        public string? SearchText { get; set; }

        public bool? IsActive { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}