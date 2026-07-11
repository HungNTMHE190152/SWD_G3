using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("FlashcardPractice")]
[Index("FlashcardId", "StudentId", Name = "UQ_FlashcardPractice", IsUnique = true)]
public partial class FlashcardPractice
{
    [Key]
    public long PracticeId { get; set; }

    public long FlashcardId { get; set; }

    public long StudentId { get; set; }

    public int? ReviewCount { get; set; }

    public int? CorrectCount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastReviewed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextReview { get; set; }

    [ForeignKey("FlashcardId")]
    [InverseProperty("FlashcardPractices")]
    public virtual Flashcard Flashcard { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("FlashcardPractices")]
    public virtual User Student { get; set; } = null!;
}
