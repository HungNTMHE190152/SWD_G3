using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("AIGenerationHistory")]
public partial class AigenerationHistory
{
    [Key]
    public long HistoryId { get; set; }

    public long PromptId { get; set; }

    public long UserId { get; set; }

    [StringLength(100)]
    public string? FeatureName { get; set; }

    public int? TokenUsed { get; set; }

    public double? ResponseTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("PromptId")]
    [InverseProperty("AigenerationHistories")]
    public virtual Aiprompt Prompt { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AigenerationHistories")]
    public virtual User User { get; set; } = null!;
}
