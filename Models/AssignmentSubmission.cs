using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("ClassroomAssignmentId", "Status", Name = "IX_AssignmentSubmissions_ClassroomAssignmentId_Status")]
[Index("StudentId", "Status", Name = "IX_AssignmentSubmissions_StudentId_Status")]
[Index("ClassroomAssignmentId", "StudentId", Name = "UQ_AssignmentSubmissions_Assignment_Student", IsUnique = true)]
public partial class AssignmentSubmission
{
    [Key]
    public long AssignmentSubmissionId { get; set; }

    public long ClassroomAssignmentId { get; set; }

    public long StudentId { get; set; }

    public string? SubmissionText { get; set; }

    [StringLength(1000)]
    public string? FileUrl { get; set; }

    [Precision(0)]
    public DateTime? SubmittedAt { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ClassroomAssignmentId")]
    [InverseProperty("AssignmentSubmissions")]
    public virtual ClassroomAssignment ClassroomAssignment { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("AssignmentSubmissions")]
    public virtual User Student { get; set; } = null!;

    [InverseProperty("AssignmentSubmission")]
    public virtual SubmissionGrade? SubmissionGrade { get; set; }
}
