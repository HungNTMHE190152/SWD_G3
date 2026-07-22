namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewModuleViewModel
    {
        public long ModuleId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public List<CourseReviewLessonViewModel> Lessons { get; set; }
            = new();
    }
}