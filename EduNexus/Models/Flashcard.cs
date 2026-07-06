using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Flashcard")]
public partial class Flashcard
{
    [Key]
    public long FlashcardId { get; set; }

    public long FlashcardSetId { get; set; }

    public string FrontContent { get; set; } = null!;

    public string BackContent { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Flashcard")]
    public virtual ICollection<FlashcardPractice> FlashcardPractices { get; set; } = new List<FlashcardPractice>();

    [ForeignKey("FlashcardSetId")]
    [InverseProperty("Flashcards")]
    public virtual FlashcardSet FlashcardSet { get; set; } = null!;
}
