using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Assignment
{
    public class SubmissionGradeViewModel
    {
        public long SubmissionId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public long ClassroomAssignmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime? SubmittedAt { get; set; }
        public string? SubmissionText { get; set; }
        public string? FileUrl { get; set; }
        [Required]
        [Range(0, 10)]
        public double Score { get; set; }
        [StringLength(1000)]
        public string? Feedback { get; set; }
    }
}