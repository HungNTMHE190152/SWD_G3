namespace EduNexus.Areas.Admin.ViewModels.Dashboard
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int ActiveAccounts { get; set; }

        public int StudentCount { get; set; }

        public int TeacherCount { get; set; }

        public int SmeCount { get; set; }

        public int PendingCourseCount { get; set; }

        public int ActiveClassroomCount { get; set; }

        public int CompletedClassroomCount { get; set; }

        public List<PendingCourseItemViewModel> RecentPendingCourses
        { get; set; } = new();
    }

    public class PendingCourseItemViewModel
    {
        public long CourseId { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string SmeName { get; set; } = string.Empty;

        public DateTime? SubmittedAt { get; set; }

        public int ModuleCount { get; set; }

        public int LessonCount { get; set; }
    }
}