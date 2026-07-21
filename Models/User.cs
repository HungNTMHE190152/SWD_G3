using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("Email", Name = "UQ_Users_Email", IsUnique = true)]
public partial class User
{
    [Key]
    public long UserId { get; set; }

    [StringLength(150)]
    public string FullName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual Account? Account { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AssignmentTemplate> AssignmentTemplates { get; set; } = new List<AssignmentTemplate>();

    [InverseProperty("PublishedByNavigation")]
    public virtual ICollection<ClassroomAssignment> ClassroomAssignments { get; set; } = new List<ClassroomAssignment>();

    [InverseProperty("PublishedByNavigation")]
    public virtual ICollection<ClassroomQuiz> ClassroomQuizzes { get; set; } = new List<ClassroomQuiz>();

    [InverseProperty("Teacher")]
    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    [InverseProperty("ReviewedByNavigation")]
    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("Student")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = new List<QuestionBank>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [InverseProperty("Student")]
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<QuizTemplate> QuizTemplates { get; set; } = new List<QuizTemplate>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<RankingConfiguration> RankingConfigurations { get; set; } = new List<RankingConfiguration>();

    [InverseProperty("Teacher")]
    public virtual ICollection<SubmissionGrade> SubmissionGrades { get; set; } = new List<SubmissionGrade>();

    [InverseProperty("Student")]
    public virtual ICollection<TeacherEvaluation> TeacherEvaluations { get; set; } = new List<TeacherEvaluation>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<TeacherPerformanceConfiguration> TeacherPerformanceConfigurations { get; set; } = new List<TeacherPerformanceConfiguration>();
}
