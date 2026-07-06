using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("AIPrompt")]
public partial class Aiprompt
{
    [Key]
    public long PromptId { get; set; }

    public long UserId { get; set; }

    [StringLength(50)]
    public string PromptType { get; set; } = null!;

    public string PromptContent { get; set; } = null!;

    [Column("AIModel")]
    [StringLength(100)]
    public string? Aimodel { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Prompt")]
    public virtual ICollection<Aicontent> Aicontents { get; set; } = new List<Aicontent>();

    [InverseProperty("Prompt")]
    public virtual ICollection<Aifeedback> Aifeedbacks { get; set; } = new List<Aifeedback>();

    [InverseProperty("Prompt")]
    public virtual ICollection<AigenerationHistory> AigenerationHistories { get; set; } = new List<AigenerationHistory>();

    [InverseProperty("Prompt")]
    public virtual ICollection<AiusageLog> AiusageLogs { get; set; } = new List<AiusageLog>();

    [ForeignKey("UserId")]
    [InverseProperty("Aiprompts")]
    public virtual User User { get; set; } = null!;
}
