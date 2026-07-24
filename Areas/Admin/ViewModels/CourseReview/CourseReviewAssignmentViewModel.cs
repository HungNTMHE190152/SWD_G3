namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewAssignmentViewModel
    {
        public long AssignmentTemplateId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Instructions { get; set; }

        public decimal TotalScore { get; set; }

        public bool IsActive { get; set; }

        public bool HasRubric { get; set; }

        public int RubricCriterionCount { get; set; }
    }
}