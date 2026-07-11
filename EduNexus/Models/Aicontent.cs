using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("AIContent")]
public partial class Aicontent
{
    [Key]
    [Column("AIContentId")]
    public long AicontentId { get; set; }

    public long PromptId { get; set; }

    [StringLength(50)]
    public string? ContentType { get; set; }

    public string? GeneratedContent { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    public long? TargetId { get; set; }

    public long? EditedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("EditedBy")]
    [InverseProperty("Aicontents")]
    public virtual User? EditedByNavigation { get; set; }

    [ForeignKey("PromptId")]
    [InverseProperty("Aicontents")]
    public virtual Aiprompt Prompt { get; set; } = null!;
}
