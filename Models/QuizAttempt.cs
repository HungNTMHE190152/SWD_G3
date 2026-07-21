using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("ClassroomQuizId", "Status", Name = "IX_QuizAttempts_ClassroomQuizId_Status")]
[Index("StudentId", "Status", Name = "IX_QuizAttempts_StudentId_Status")]
[Index("ClassroomQuizId", "StudentId", "AttemptNumber", Name = "UQ_QuizAttempts_Quiz_Student_Attempt", IsUnique = true)]
public partial class QuizAttempt
{
    [Key]
    public long QuizAttemptId { get; set; }

    public long ClassroomQuizId { get; set; }

    public long StudentId { get; set; }

    public int AttemptNumber { get; set; }

    [Precision(0)]
    public DateTime StartTime { get; set; }

    [Precision(0)]
    public DateTime? SubmitTime { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? TotalScore { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [ForeignKey("ClassroomQuizId")]
    [InverseProperty("QuizAttempts")]
    public virtual ClassroomQuiz ClassroomQuiz { get; set; } = null!;

    [InverseProperty("QuizAttempt")]
    public virtual ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();

    [ForeignKey("StudentId")]
    [InverseProperty("QuizAttempts")]
    public virtual User Student { get; set; } = null!;
}
