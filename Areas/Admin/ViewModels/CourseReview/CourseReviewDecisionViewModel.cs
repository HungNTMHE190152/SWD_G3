using System.ComponentModel.DataAnnotations;

namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewDecisionViewModel
    {
        [Range(
            1,
            long.MaxValue,
            ErrorMessage = "Invalid course.")]
        public long CourseId { get; set; }

        [StringLength(
            1000,
            ErrorMessage =
                "Review comment cannot exceed 1000 characters.")]
        public string? ReviewComment { get; set; }
    }
}