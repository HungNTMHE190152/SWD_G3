using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("EssaySubmission")]
[Index("AssignmentId", "StudentId", Name = "UQ_EssaySubmission", IsUnique = true)]
public partial class EssaySubmission
{
    [Key]
    public long SubmissionId { get; set; }

    public long AssignmentId { get; set; }

    public long StudentId { get; set; }

    public string? SubmissionText { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SubmittedAt { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    [StringLength(30)]
    public string? GradingStatus { get; set; }

    [Column("AIJobId")]
    [StringLength(100)]
    [Unicode(false)]
    public string? AijobId { get; set; }

    [InverseProperty("Submission")]
    public virtual ICollection<Aifeedback> Aifeedbacks { get; set; } = new List<Aifeedback>();

    [ForeignKey("AssignmentId")]
    [InverseProperty("EssaySubmissions")]
    public virtual Assignment Assignment { get; set; } = null!;

    [InverseProperty("Submission")]
    public virtual EssayResult? EssayResult { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("EssaySubmissions")]
    public virtual User Student { get; set; } = null!;

    [InverseProperty("Submission")]
    public virtual ICollection<SubmissionAttachment> SubmissionAttachments { get; set; } = new List<SubmissionAttachment>();
}
