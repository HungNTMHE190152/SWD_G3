using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("AIUsageLog")]
public partial class AiusageLog
{
    [Key]
    public long LogId { get; set; }

    public long UserId { get; set; }

    public long? PromptId { get; set; }

    [StringLength(100)]
    public string? FeatureName { get; set; }

    public int? TokenInput { get; set; }

    public int? TokenOutput { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? EstimatedCost { get; set; }

    public double? ResponseTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("PromptId")]
    [InverseProperty("AiusageLogs")]
    public virtual Aiprompt? Prompt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AiusageLogs")]
    public virtual User User { get; set; } = null!;
}
