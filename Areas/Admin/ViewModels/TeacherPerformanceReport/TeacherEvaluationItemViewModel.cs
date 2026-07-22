namespace EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport
{
    public class TeacherEvaluationItemViewModel
    {
        public long TeacherEvaluationId { get; set; }

        public long ClassroomId { get; set; }

        public string ClassroomName { get; set; } =
            string.Empty;

        public long StudentId { get; set; }

        public string StudentName { get; set; } =
            string.Empty;

        public string StudentEmail { get; set; } =
            string.Empty;

        public byte ContentClarityRating { get; set; }

        public byte SupportRating { get; set; }

        public byte FeedbackQualityRating { get; set; }

        public byte ClassOrganizationRating { get; set; }

        public byte OverallRating { get; set; }

        public string? Comment { get; set; }

        public bool IsCommentHidden { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}