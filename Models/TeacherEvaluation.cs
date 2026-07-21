using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("ClassroomId", Name = "IX_TeacherEvaluations_ClassroomId")]
[Index("ClassroomId", "StudentId", Name = "UQ_TeacherEvaluations_Classroom_Student", IsUnique = true)]
public partial class TeacherEvaluation
{
    [Key]
    public long TeacherEvaluationId { get; set; }

    public long ClassroomId { get; set; }

    public long StudentId { get; set; }

    public byte ContentClarityRating { get; set; }

    public byte SupportRating { get; set; }

    public byte FeedbackQualityRating { get; set; }

    public byte ClassOrganizationRating { get; set; }

    public byte OverallRating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    public bool IsCommentHidden { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ClassroomId")]
    [InverseProperty("TeacherEvaluations")]
    public virtual Classroom Classroom { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("TeacherEvaluations")]
    public virtual User Student { get; set; } = null!;
}
