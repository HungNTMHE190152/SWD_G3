using System;

namespace EduNexus.ViewModels.Assignment
{
    public class AssignmentSubmissionViewModel
    {
        public long SubmissionId { get; set; }

        public long ClassroomAssignmentId { get; set; }

        public string AssignmentTitle { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public DateTime? SubmittedAt { get; set; }
        public string Status { get; set; } = string.Empty;

        public double? Score { get; set; }

        public bool IsLate { get; set; }
    }
}