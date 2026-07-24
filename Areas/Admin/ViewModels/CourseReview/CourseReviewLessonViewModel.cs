namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewLessonViewModel
    {
        public long LessonId { get; set; }

        public long ModuleId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        public string LessonType { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public string Status { get; set; } = string.Empty;

        public int? EstimatedMinutes { get; set; }

        public int ResourceCount { get; set; }
    }
}