using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("User")]
[Index("Email", Name = "UQ__User__A9D105343B0AC707", IsUnique = true)]
public partial class User
{
    [Key]
    public long UserId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(300)]
    public string? Avatar { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    public bool Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [InverseProperty("EditedByNavigation")]
    public virtual ICollection<Aicontent> Aicontents { get; set; } = new List<Aicontent>();

    [InverseProperty("User")]
    public virtual ICollection<AigenerationHistory> AigenerationHistories { get; set; } = new List<AigenerationHistory>();

    [InverseProperty("User")]
    public virtual ICollection<Aiprompt> Aiprompts { get; set; } = new List<Aiprompt>();

    [InverseProperty("User")]
    public virtual ICollection<AiusageLog> AiusageLogs { get; set; } = new List<AiusageLog>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CourseGroup> CourseGroups { get; set; } = new List<CourseGroup>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("Student")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Grader")]
    public virtual ICollection<EssayResult> EssayResults { get; set; } = new List<EssayResult>();

    [InverseProperty("Student")]
    public virtual ICollection<EssaySubmission> EssaySubmissions { get; set; } = new List<EssaySubmission>();

    [InverseProperty("ImportedByNavigation")]
    public virtual ICollection<FlashcardImportHistory> FlashcardImportHistories { get; set; } = new List<FlashcardImportHistory>();

    [InverseProperty("Student")]
    public virtual ICollection<FlashcardPractice> FlashcardPractices { get; set; } = new List<FlashcardPractice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<FlashcardSet> FlashcardSets { get; set; } = new List<FlashcardSet>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = new List<QuestionBank>();

    [InverseProperty("ImportedByNavigation")]
    public virtual ICollection<QuestionImportHistory> QuestionImportHistories { get; set; } = new List<QuestionImportHistory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [InverseProperty("Student")]
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
