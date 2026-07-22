namespace EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport
{
    public class TeacherPerformanceDetailsViewModel
    {
        public TeacherPerformanceReportItemViewModel Summary
        { get; set; } = new();

        public List<TeacherPerformanceClassroomViewModel>
            Classrooms
        { get; set; } = new();

        public List<TeacherEvaluationItemViewModel>
            Evaluations
        { get; set; } = new();

        public decimal FeedbackWeightPercentage { get; set; }

        public decimal CompletionWeightPercentage { get; set; }

        public decimal StudentPerformanceWeightPercentage
        { get; set; }

        public decimal AssignmentWeightPercentage { get; set; }

        public decimal QuizWeightPercentage { get; set; }

        public decimal LessonProgressWeightPercentage
        { get; set; }

        public decimal AverageContentClarityRating { get; set; }

        public decimal AverageSupportRating { get; set; }

        public decimal AverageFeedbackQualityRating { get; set; }

        public decimal AverageClassOrganizationRating { get; set; }

        public decimal AverageOverallRating { get; set; }
    }
}