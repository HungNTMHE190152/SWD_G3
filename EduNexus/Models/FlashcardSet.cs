using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("FlashcardSet")]
public partial class FlashcardSet
{
    [Key]
    public long FlashcardSetId { get; set; }

    public long CourseId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(200)]
    public string SetName { get; set; } = null!;

    public string? Description { get; set; }

    [Column("IsAIGenerated")]
    public bool? IsAigenerated { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("FlashcardSets")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("FlashcardSets")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("FlashcardSet")]
    public virtual ICollection<FlashcardImportHistory> FlashcardImportHistories { get; set; } = new List<FlashcardImportHistory>();

    [InverseProperty("FlashcardSet")]
    public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}
