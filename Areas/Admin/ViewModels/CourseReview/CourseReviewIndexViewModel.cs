namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewIndexViewModel
    {
        public CourseReviewFilterViewModel Filter { get; set; }
            = new();

        public List<CourseReviewListItemViewModel> Courses
        { get; set; } = new();

        public List<CourseReviewCategoryOptionViewModel> Categories
        { get; set; } = new();

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int PendingCount { get; set; }

        public int ApprovedCount { get; set; }

        public int RejectedCount { get; set; }

        public bool HasPreviousPage =>
            Filter.Page > 1;

        public bool HasNextPage =>
            Filter.Page < TotalPages;
    }
}