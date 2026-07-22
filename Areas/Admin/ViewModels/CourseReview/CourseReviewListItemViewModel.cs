namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewListItemViewModel
    {
        public long CourseId { get; set; }

        public string CourseCode { get; set; } =
            string.Empty;

        public string Title { get; set; } =
            string.Empty;

        public string CategoryName { get; set; } =
            string.Empty;

        public string SmeName { get; set; } =
            string.Empty;

        public string Status { get; set; } =
            string.Empty;

        public string? ThumbnailUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public int ModuleCount { get; set; }

        public int LessonCount { get; set; }

        public int QuizTemplateCount { get; set; }

        public int AssignmentTemplateCount { get; set; }
    }
}