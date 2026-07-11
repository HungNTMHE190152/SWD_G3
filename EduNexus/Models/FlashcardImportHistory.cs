using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("FlashcardImportHistory")]
public partial class FlashcardImportHistory
{
    [Key]
    public long ImportId { get; set; }

    public long FlashcardSetId { get; set; }

    public long ImportedBy { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = null!;

    public int? TotalCards { get; set; }

    public int? SuccessCards { get; set; }

    public int? FailedCards { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ImportedAt { get; set; }

    [ForeignKey("FlashcardSetId")]
    [InverseProperty("FlashcardImportHistories")]
    public virtual FlashcardSet FlashcardSet { get; set; } = null!;

    [ForeignKey("ImportedBy")]
    [InverseProperty("FlashcardImportHistories")]
    public virtual User ImportedByNavigation { get; set; } = null!;
}
