namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewFilterViewModel
    {
        public string? SearchText { get; set; }

        public string? Status { get; set; }

        public long? CategoryId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}