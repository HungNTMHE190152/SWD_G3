using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("AssignmentSubmissionId", Name = "UQ_SubmissionGrades_Submission", IsUnique = true)]
public partial class SubmissionGrade
{
    [Key]
    public long SubmissionGradeId { get; set; }

    public long AssignmentSubmissionId { get; set; }

    public long TeacherId { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal Score { get; set; }

    public string? Feedback { get; set; }

    [Precision(0)]
    public DateTime GradedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("AssignmentSubmissionId")]
    [InverseProperty("SubmissionGrade")]
    public virtual AssignmentSubmission AssignmentSubmission { get; set; } = null!;

    [InverseProperty("SubmissionGrade")]
    public virtual ICollection<SubmissionRubricScore> SubmissionRubricScores { get; set; } = new List<SubmissionRubricScore>();

    [ForeignKey("TeacherId")]
    [InverseProperty("SubmissionGrades")]
    public virtual User Teacher { get; set; } = null!;
}
