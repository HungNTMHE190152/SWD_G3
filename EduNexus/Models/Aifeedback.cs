using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("AIFeedback")]
public partial class Aifeedback
{
    [Key]
    public long FeedbackId { get; set; }

    public long SubmissionId { get; set; }

    public long PromptId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? SuggestedScore { get; set; }

    public string? SuggestedFeedback { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? ConfidenceScore { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? GeneratedAt { get; set; }

    [ForeignKey("PromptId")]
    [InverseProperty("Aifeedbacks")]
    public virtual Aiprompt Prompt { get; set; } = null!;

    [ForeignKey("SubmissionId")]
    [InverseProperty("Aifeedbacks")]
    public virtual EssaySubmission Submission { get; set; } = null!;
}
