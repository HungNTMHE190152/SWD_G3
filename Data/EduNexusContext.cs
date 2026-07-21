using System;
using System.Collections.Generic;
using EduNexus.Models;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Data;

public partial class EduNexusContext : DbContext
{
    public EduNexusContext(DbContextOptions<EduNexusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

    public virtual DbSet<AssignmentTemplate> AssignmentTemplates { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<ClassroomAssignment> ClassroomAssignments { get; set; }

    public virtual DbSet<ClassroomQuiz> ClassroomQuizzes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseReview> CourseReviews { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonProgress> LessonProgresses { get; set; }

    public virtual DbSet<LessonResource> LessonResources { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionBank> QuestionBanks { get; set; }

    public virtual DbSet<QuizAnswer> QuizAnswers { get; set; }

    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

    public virtual DbSet<QuizTemplate> QuizTemplates { get; set; }

    public virtual DbSet<QuizTemplateQuestion> QuizTemplateQuestions { get; set; }

    public virtual DbSet<RankingConfiguration> RankingConfigurations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rubric> Rubrics { get; set; }

    public virtual DbSet<RubricCriterion> RubricCriteria { get; set; }

    public virtual DbSet<SubmissionGrade> SubmissionGrades { get; set; }

    public virtual DbSet<SubmissionRubricScore> SubmissionRubricScores { get; set; }

    public virtual DbSet<TeacherEvaluation> TeacherEvaluations { get; set; }

    public virtual DbSet<TeacherPerformanceConfiguration> TeacherPerformanceConfigurations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Roles");

            entity.HasOne(d => d.User).WithOne(p => p.Account)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Users");
        });

        modelBuilder.Entity<AssignmentSubmission>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");

            entity.HasOne(d => d.ClassroomAssignment).WithMany(p => p.AssignmentSubmissions).HasConstraintName("FK_AssignmentSubmissions_ClassroomAssignments");

            entity.HasOne(d => d.Student).WithMany(p => p.AssignmentSubmissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignmentSubmissions_Student");
        });

        modelBuilder.Entity<AssignmentTemplate>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TotalScore).HasDefaultValue(10.00m);

            entity.HasOne(d => d.Course).WithMany(p => p.AssignmentTemplates).HasConstraintName("FK_AssignmentTemplates_Courses");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssignmentTemplates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignmentTemplates_CreatedBy");

            entity.HasOne(d => d.Module).WithMany(p => p.AssignmentTemplates).HasConstraintName("FK_AssignmentTemplates_Modules");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasOne(d => d.Question).WithMany(p => p.Choices).HasConstraintName("FK_Choices_Questions");
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");

            entity.HasOne(d => d.Course).WithMany(p => p.Classrooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Classrooms_Courses");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classrooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Classrooms_Teacher");
        });

        modelBuilder.Entity<ClassroomAssignment>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");

            entity.HasOne(d => d.AssignmentTemplate).WithMany(p => p.ClassroomAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassroomAssignments_AssignmentTemplates");

            entity.HasOne(d => d.Classroom).WithMany(p => p.ClassroomAssignments).HasConstraintName("FK_ClassroomAssignments_Classrooms");

            entity.HasOne(d => d.PublishedByNavigation).WithMany(p => p.ClassroomAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassroomAssignments_PublishedBy");
        });

        modelBuilder.Entity<ClassroomQuiz>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.MaxAttempts).HasDefaultValue(1);
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");

            entity.HasOne(d => d.Classroom).WithMany(p => p.ClassroomQuizzes).HasConstraintName("FK_ClassroomQuizzes_Classrooms");

            entity.HasOne(d => d.PublishedByNavigation).WithMany(p => p.ClassroomQuizzes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassroomQuizzes_PublishedBy");

            entity.HasOne(d => d.QuizTemplate).WithMany(p => p.ClassroomQuizzes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassroomQuizzes_QuizTemplates");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Courses_Categories");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Courses_CreatedBy");
        });

        modelBuilder.Entity<CourseReview>(entity =>
        {
            entity.Property(e => e.ReviewedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseReviews_Courses");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.CourseReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseReviews_ReviewedBy");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.EnrolledAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.Classroom).WithMany(p => p.Enrollments).HasConstraintName("FK_Enrollments_Classrooms");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Student");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.LessonType).HasDefaultValue("TEXT");
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");

            entity.HasOne(d => d.Module).WithMany(p => p.Lessons).HasConstraintName("FK_Lessons_Modules");
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.HasOne(d => d.Enrollment).WithMany(p => p.LessonProgresses).HasConstraintName("FK_LessonProgress_Enrollments");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LessonProgress_Lessons");
        });

        modelBuilder.Entity<LessonResource>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonResources).HasConstraintName("FK_LessonResources_Lessons");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Course).WithMany(p => p.Modules).HasConstraintName("FK_Modules_Courses");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Difficulty).HasDefaultValue("EASY");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions_CreatedBy");

            entity.HasOne(d => d.QuestionBank).WithMany(p => p.Questions).HasConstraintName("FK_Questions_QuestionBanks");
        });

        modelBuilder.Entity<QuestionBank>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Course).WithMany(p => p.QuestionBanks).HasConstraintName("FK_QuestionBanks_Courses");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.QuestionBanks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionBanks_CreatedBy");
        });

        modelBuilder.Entity<QuizAnswer>(entity =>
        {
            entity.HasOne(d => d.Choice).WithMany(p => p.QuizAnswers).HasConstraintName("FK_QuizAnswers_Choices");

            entity.HasOne(d => d.Question).WithMany(p => p.QuizAnswers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizAnswers_Questions");

            entity.HasOne(d => d.QuizAttempt).WithMany(p => p.QuizAnswers).HasConstraintName("FK_QuizAnswers_QuizAttempts");
        });

        modelBuilder.Entity<QuizAttempt>(entity =>
        {
            entity.Property(e => e.StartTime).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("IN_PROGRESS");

            entity.HasOne(d => d.ClassroomQuiz).WithMany(p => p.QuizAttempts).HasConstraintName("FK_QuizAttempts_ClassroomQuizzes");

            entity.HasOne(d => d.Student).WithMany(p => p.QuizAttempts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizAttempts_Student");
        });

        modelBuilder.Entity<QuizTemplate>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DefaultDurationMinutes).HasDefaultValue(30);
            entity.Property(e => e.DefaultMaxAttempts).HasDefaultValue(1);
            entity.Property(e => e.DefaultPassingScore).HasDefaultValue(5.00m);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Course).WithMany(p => p.QuizTemplates).HasConstraintName("FK_QuizTemplates_Courses");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.QuizTemplates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizTemplates_CreatedBy");

            entity.HasOne(d => d.Module).WithMany(p => p.QuizTemplates).HasConstraintName("FK_QuizTemplates_Modules");
        });

        modelBuilder.Entity<QuizTemplateQuestion>(entity =>
        {
            entity.Property(e => e.Score).HasDefaultValue(1.00m);

            entity.HasOne(d => d.Question).WithMany(p => p.QuizTemplateQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizTemplateQuestions_Questions");

            entity.HasOne(d => d.QuizTemplate).WithMany(p => p.QuizTemplateQuestions).HasConstraintName("FK_QuizTemplateQuestions_QuizTemplates");
        });

        modelBuilder.Entity<RankingConfiguration>(entity =>
        {
            entity.HasIndex(e => e.IsActive, "UX_RankingConfigurations_OneActive")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.RankingConfigurations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RankingConfigurations_UpdatedBy");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Rubric>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AssignmentTemplate).WithOne(p => p.Rubric).HasConstraintName("FK_Rubrics_AssignmentTemplates");
        });

        modelBuilder.Entity<RubricCriterion>(entity =>
        {
            entity.HasOne(d => d.Rubric).WithMany(p => p.RubricCriteria).HasConstraintName("FK_RubricCriteria_Rubrics");
        });

        modelBuilder.Entity<SubmissionGrade>(entity =>
        {
            entity.Property(e => e.GradedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AssignmentSubmission).WithOne(p => p.SubmissionGrade).HasConstraintName("FK_SubmissionGrades_AssignmentSubmissions");

            entity.HasOne(d => d.Teacher).WithMany(p => p.SubmissionGrades)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmissionGrades_Teacher");
        });

        modelBuilder.Entity<SubmissionRubricScore>(entity =>
        {
            entity.HasOne(d => d.RubricCriterion).WithMany(p => p.SubmissionRubricScores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmissionRubricScores_RubricCriteria");

            entity.HasOne(d => d.SubmissionGrade).WithMany(p => p.SubmissionRubricScores).HasConstraintName("FK_SubmissionRubricScores_SubmissionGrades");
        });

        modelBuilder.Entity<TeacherEvaluation>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Classroom).WithMany(p => p.TeacherEvaluations).HasConstraintName("FK_TeacherEvaluations_Classrooms");

            entity.HasOne(d => d.Student).WithMany(p => p.TeacherEvaluations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherEvaluations_Student");
        });

        modelBuilder.Entity<TeacherPerformanceConfiguration>(entity =>
        {
            entity.HasIndex(e => e.IsActive, "UX_TeacherPerformanceConfigurations_OneActive")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.MinimumEvaluationCount).HasDefaultValue(5);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TeacherPerformanceConfigurations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherPerformanceConfigurations_UpdatedBy");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
