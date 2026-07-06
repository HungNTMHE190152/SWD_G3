using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("EssayResult")]
[Index("SubmissionId", Name = "UQ_EssayResult", IsUnique = true)]
public partial class EssayResult
{
    [Key]
    public long ResultId { get; set; }

    public long SubmissionId { get; set; }

    public long GraderId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? TotalScore { get; set; }

    public string? Feedback { get; set; }

    public bool? IsPublished { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? GradedAt { get; set; }

    [ForeignKey("GraderId")]
    [InverseProperty("EssayResults")]
    public virtual User Grader { get; set; } = null!;

    [InverseProperty("Result")]
    public virtual ICollection<RubricScore> RubricScores { get; set; } = new List<RubricScore>();

    [ForeignKey("SubmissionId")]
    [InverseProperty("EssayResult")]
    public virtual EssaySubmission Submission { get; set; } = null!;
}
