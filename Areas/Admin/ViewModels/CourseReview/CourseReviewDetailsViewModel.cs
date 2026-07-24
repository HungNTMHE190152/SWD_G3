using EduNexus.Constants;

namespace EduNexus.Areas.Admin.ViewModels.CourseReview
{
    public class CourseReviewDetailsViewModel
    {
        public long CourseId { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string Status { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string SmeName { get; set; } = string.Empty;

        public string SmeEmail { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public List<CourseReviewModuleViewModel> Modules { get; set; }
            = new();

        public List<CourseReviewQuizViewModel> QuizTemplates
        { get; set; } = new();

        public List<CourseReviewAssignmentViewModel>
            AssignmentTemplates
        { get; set; } = new();

        public List<CourseReviewHistoryItemViewModel> ReviewHistory
        { get; set; } = new();

        public List<string> ContentWarnings { get; set; }
            = new();

        public int ModuleCount =>
            Modules.Count;

        public int LessonCount =>
            Modules.Sum(module => module.Lessons.Count);

        public int PublishedLessonCount =>
            Modules.Sum(module =>
                module.Lessons.Count(lesson =>
                    lesson.Status == "PUBLISHED"));

        public int DraftLessonCount =>
            Modules.Sum(module =>
                module.Lessons.Count(lesson =>
                    lesson.Status == "DRAFT"));

        public int QuizTemplateCount =>
            QuizTemplates.Count;

        public int AssignmentTemplateCount =>
            AssignmentTemplates.Count;

        public bool CanReview =>
            Status == CourseStatuses.PendingReview;
    }
}