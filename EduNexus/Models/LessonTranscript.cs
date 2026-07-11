using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("LessonTranscript")]
public partial class LessonTranscript
{
    [Key]
    public long TranscriptId { get; set; }

    public long LessonId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? YoutubeVideoId { get; set; }

    public string? RawTranscript { get; set; }

    public string? FormattedSummary { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("LessonTranscripts")]
    public virtual Lesson Lesson { get; set; } = null!;
}
