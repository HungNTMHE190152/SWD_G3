namespace EduNexus.ViewModels.Assignment;

public class EnrollmentViewModel
{
    public long EnrollmentId { get; set; }

    public long ClassroomId { get; set; }

    public string ClassroomName { get; set; } = string.Empty;

    public long StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string StudentEmail { get; set; } = string.Empty;

    public DateTime EnrolledAt { get; set; }

    public string Status { get; set; } = string.Empty;
}