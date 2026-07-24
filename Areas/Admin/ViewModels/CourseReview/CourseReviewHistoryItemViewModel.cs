namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewHistoryItemViewModel
    {
        public long CourseReviewId { get; set; }

        public string Decision { get; set; } = string.Empty;

        public string? ReviewComment { get; set; }

        public string ReviewerName { get; set; } = string.Empty;

        public DateTime ReviewedAt { get; set; }
    }
}