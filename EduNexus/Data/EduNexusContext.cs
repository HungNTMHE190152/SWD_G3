using System;
using System.Collections.Generic;
using EduNexus.Models;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Data;

public partial class EduNexusContext : DbContext
{
    public EduNexusContext()
    {
    }

    public EduNexusContext(DbContextOptions<EduNexusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Aicontent> Aicontents { get; set; }

    public virtual DbSet<Aifeedback> Aifeedbacks { get; set; }

    public virtual DbSet<AigenerationHistory> AigenerationHistories { get; set; }

    public virtual DbSet<Aiprompt> Aiprompts { get; set; }

    public virtual DbSet<AiusageLog> AiusageLogs { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AssignmentQuestion> AssignmentQuestions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseGroup> CourseGroups { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<EssayResult> EssayResults { get; set; }

    public virtual DbSet<EssaySubmission> EssaySubmissions { get; set; }

    public virtual DbSet<Flashcard> Flashcards { get; set; }

    public virtual DbSet<FlashcardImportHistory> FlashcardImportHistories { get; set; }

    public virtual DbSet<FlashcardPractice> FlashcardPractices { get; set; }

    public virtual DbSet<FlashcardSet> FlashcardSets { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonTranscript> LessonTranscripts { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Progress> Progresses { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionBank> QuestionBanks { get; set; }

    public virtual DbSet<QuestionImportHistory> QuestionImportHistories { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizAnswer> QuizAnswers { get; set; }

    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

    public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rubric> Rubrics { get; set; }

    public virtual DbSet<RubricCriterion> RubricCriteria { get; set; }

    public virtual DbSet<RubricScore> RubricScores { get; set; }

    public virtual DbSet<SubmissionAttachment> SubmissionAttachments { get; set; }

    public virtual DbSet<SystemConfig> SystemConfigs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=THEHUNG\\MSSQLSERVER01;Database=AI_LMS;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A68DA3F6E9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Provider).HasDefaultValue("LOCAL");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_User");
        });

        modelBuilder.Entity<Aicontent>(entity =>
        {
            entity.HasKey(e => e.AicontentId).HasName("PK__AIConten__D067E6EC84CBF8BC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.Aicontents).HasConstraintName("FK_AIContent_User");

            entity.HasOne(d => d.Prompt).WithMany(p => p.Aicontents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIContent_Prompt");
        });

        modelBuilder.Entity<Aifeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__AIFeedba__6A4BEDD6F26F0057");

            entity.Property(e => e.GeneratedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Prompt).WithMany(p => p.Aifeedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIFeedback_Prompt");

            entity.HasOne(d => d.Submission).WithMany(p => p.Aifeedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIFeedback_Submission");
        });

        modelBuilder.Entity<AigenerationHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__AIGenera__4D7B4ABDB10C721B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TokenUsed).HasDefaultValue(0);

            entity.HasOne(d => d.Prompt).WithMany(p => p.AigenerationHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIGenerationHistory_Prompt");

            entity.HasOne(d => d.User).WithMany(p => p.AigenerationHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIGenerationHistory_User");
        });

        modelBuilder.Entity<Aiprompt>(entity =>
        {
            entity.HasKey(e => e.PromptId).HasName("PK__AIPrompt__456CA75300D59382");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Aiprompts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIPrompt_User");
        });

        modelBuilder.Entity<AiusageLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AIUsageL__5E548648D6C22365");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TokenInput).HasDefaultValue(0);
            entity.Property(e => e.TokenOutput).HasDefaultValue(0);

            entity.HasOne(d => d.Prompt).WithMany(p => p.AiusageLogs).HasConstraintName("FK_AIUsageLog_Prompt");

            entity.HasOne(d => d.User).WithMany(p => p.AiusageLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIUsageLog_User");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__Assignme__32499E773E1E52AC");

            entity.Property(e => e.AllowLateSubmission).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAigenerated).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");
            entity.Property(e => e.TotalScore).HasDefaultValue(10m);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_User");

            entity.HasOne(d => d.Module).WithMany(p => p.Assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Module");
        });

        modelBuilder.Entity<AssignmentQuestion>(entity =>
        {
            entity.HasKey(e => e.AssignmentQuestionId).HasName("PK__Assignme__6168046051929CAF");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);
            entity.Property(e => e.Score).HasDefaultValue(1m);

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignmentQuestion_Assignment");

            entity.HasOne(d => d.Question).WithMany(p => p.AssignmentQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignmentQuestion_Question");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B10390BBA");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.ChoiceId).HasName("PK__Choice__76F516A6BD94FEBA");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);
            entity.Property(e => e.IsCorrect).HasDefaultValue(false);

            entity.HasOne(d => d.Question).WithMany(p => p.Choices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Choice_Question");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A7A8C274CA");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Price).HasDefaultValue(0m);
            entity.Property(e => e.Status).HasDefaultValue("Draft");

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Category");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_User");
        });

        modelBuilder.Entity<CourseGroup>(entity =>
        {
            entity.HasKey(e => e.CourseGroupId).HasName("PK__CourseGr__E9E863F0CEBD77CF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CourseGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseGroup_User");

            entity.HasMany(d => d.Courses).WithMany(p => p.CourseGroups)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseGroupDetail",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CourseGroupDetail_Course"),
                    l => l.HasOne<CourseGroup>().WithMany()
                        .HasForeignKey("CourseGroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CourseGroupDetail_Group"),
                    j =>
                    {
                        j.HasKey("CourseGroupId", "CourseId").HasName("PK__CourseGr__857AB4EA26A5268D");
                        j.ToTable("CourseGroupDetail");
                    });
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F68771B2DB11E3A");

            entity.HasIndex(e => new { e.CourseId, e.StudentId }, "UQ_Enroll_Course")
                .IsUnique()
                .HasFilter("([CourseId] IS NOT NULL)");

            entity.HasIndex(e => new { e.CourseGroupId, e.StudentId }, "UQ_Enroll_Group")
                .IsUnique()
                .HasFilter("([CourseGroupId] IS NOT NULL)");

            entity.Property(e => e.EnrollDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Progress).HasDefaultValue(0m);
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.CourseGroup).WithMany(p => p.Enrollments).HasConstraintName("FK_Enrollment_CourseGroup");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments).HasConstraintName("FK_Enrollment_Course");

            entity.HasOne(d => d.ParentEnrollment).WithMany(p => p.InverseParentEnrollment).HasConstraintName("FK_Enrollment_Parent");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Student");
        });

        modelBuilder.Entity<EssayResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__EssayRes__976902080C345CA4");

            entity.Property(e => e.GradedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPublished).HasDefaultValue(false);

            entity.HasOne(d => d.Grader).WithMany(p => p.EssayResults)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EssayResult_Grader");

            entity.HasOne(d => d.Submission).WithOne(p => p.EssayResult)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EssayResult_Submission");
        });

        modelBuilder.Entity<EssaySubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__EssaySub__449EE125EB72C489");

            entity.Property(e => e.GradingStatus).HasDefaultValue("PENDING");
            entity.Property(e => e.Status).HasDefaultValue("SUBMITTED");
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Assignment).WithMany(p => p.EssaySubmissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EssaySubmission_Assignment");

            entity.HasOne(d => d.Student).WithMany(p => p.EssaySubmissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EssaySubmission_Student");
        });

        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasKey(e => e.FlashcardId).HasName("PK__Flashcar__D36F857256153973");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);

            entity.HasOne(d => d.FlashcardSet).WithMany(p => p.Flashcards)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flashcard_Set");
        });

        modelBuilder.Entity<FlashcardImportHistory>(entity =>
        {
            entity.HasKey(e => e.ImportId).HasName("PK__Flashcar__869767EA61BDE4CF");

            entity.Property(e => e.FailedCards).HasDefaultValue(0);
            entity.Property(e => e.ImportedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SuccessCards).HasDefaultValue(0);
            entity.Property(e => e.TotalCards).HasDefaultValue(0);

            entity.HasOne(d => d.FlashcardSet).WithMany(p => p.FlashcardImportHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlashcardImport_Set");

            entity.HasOne(d => d.ImportedByNavigation).WithMany(p => p.FlashcardImportHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlashcardImport_User");
        });

        modelBuilder.Entity<FlashcardPractice>(entity =>
        {
            entity.HasKey(e => e.PracticeId).HasName("PK__Flashcar__352A17F2CD7429D4");

            entity.Property(e => e.CorrectCount).HasDefaultValue(0);
            entity.Property(e => e.ReviewCount).HasDefaultValue(0);

            entity.HasOne(d => d.Flashcard).WithMany(p => p.FlashcardPractices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlashcardPractice_Flashcard");

            entity.HasOne(d => d.Student).WithMany(p => p.FlashcardPractices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlashcardPractice_Student");
        });

        modelBuilder.Entity<FlashcardSet>(entity =>
        {
            entity.HasKey(e => e.FlashcardSetId).HasName("PK__Flashcar__5058513CC9A7598B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAigenerated).HasDefaultValue(false);

            entity.HasOne(d => d.Course).WithMany(p => p.FlashcardSets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlashcardSet_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FlashcardSets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlashcardSet_User");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lesson__B084ACD0DF8AC72B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Duration).HasDefaultValue(0);
            entity.Property(e => e.IsPublished).HasDefaultValue(false);

            entity.HasOne(d => d.Module).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lesson_Module");
        });

        modelBuilder.Entity<LessonTranscript>(entity =>
        {
            entity.HasKey(e => e.TranscriptId).HasName("PK__LessonTr__FD083E13D4B0FD76");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("PENDING");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonTranscripts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transcript_Lesson");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Module__2B7477A7A7D88E0D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPublished).HasDefaultValue(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Modules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Module_Course");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__Progress__BAE29CA552AD479A");

            entity.Property(e => e.CompletionPercentage).HasDefaultValue(0m);
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.LastAccess).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.Progresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Progress_Enrollment");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Progresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Progress_Lesson");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC36A02197");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAigenerated).HasDefaultValue(false);

            entity.HasOne(d => d.Bank).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_Bank");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_User");
        });

        modelBuilder.Entity<QuestionBank>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("PK__Question__AA08CB136898B2A9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPublic).HasDefaultValue(false);

            entity.HasOne(d => d.Course).WithMany(p => p.QuestionBanks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionBank_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.QuestionBanks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionBank_User");
        });

        modelBuilder.Entity<QuestionImportHistory>(entity =>
        {
            entity.HasKey(e => e.ImportId).HasName("PK__Question__869767EAFB2F86D9");

            entity.Property(e => e.FailedQuestions).HasDefaultValue(0);
            entity.Property(e => e.ImportedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SuccessQuestions).HasDefaultValue(0);
            entity.Property(e => e.TotalQuestions).HasDefaultValue(0);

            entity.HasOne(d => d.Bank).WithMany(p => p.QuestionImportHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionImport_Bank");

            entity.HasOne(d => d.ImportedByNavigation).WithMany(p => p.QuestionImportHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionImport_User");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PK__Quiz__8B42AE8ECDF4DFCE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ShuffleAnswer).HasDefaultValue(false);
            entity.Property(e => e.ShuffleQuestion).HasDefaultValue(false);

            entity.HasOne(d => d.Assignment).WithOne(p => p.Quiz)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quiz_Assignment");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Quizzes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quiz_User");
        });

        modelBuilder.Entity<QuizAnswer>(entity =>
        {
            entity.HasKey(e => e.QuizAnswerId).HasName("PK__QuizAnsw__375475467C699094");

            entity.HasOne(d => d.Attempt).WithMany(p => p.QuizAnswers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizAnswer_Attempt");

            entity.HasOne(d => d.Choice).WithMany(p => p.QuizAnswers).HasConstraintName("FK_QuizAnswer_Choice");

            entity.HasOne(d => d.Question).WithMany(p => p.QuizAnswers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizAnswer_Question");
        });

        modelBuilder.Entity<QuizAttempt>(entity =>
        {
            entity.HasKey(e => e.AttemptId).HasName("PK__QuizAtte__891A68E6EFE20FF3");

            entity.Property(e => e.AttemptNumber).HasDefaultValue(1);
            entity.Property(e => e.Status).HasDefaultValue("IN_PROGRESS");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizAttempts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizAttempt_Quiz");

            entity.HasOne(d => d.Student).WithMany(p => p.QuizAttempts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizAttempt_Student");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.QuizQuestionId).HasName("PK__QuizQues__45E34D3E173E27C1");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);
            entity.Property(e => e.Score).HasDefaultValue(1m);

            entity.HasOne(d => d.Question).WithMany(p => p.QuizQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizQuestion_Question");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizQuestion_Quiz");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__Resource__4ED1816FF929FC74");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Resources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resource_Lesson");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AB4BAEEFE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Rubric>(entity =>
        {
            entity.HasKey(e => e.RubricId).HasName("PK__Rubric__8F6D0781987C4BDB");

            entity.Property(e => e.TotalWeight).HasDefaultValue(100m);

            entity.HasOne(d => d.Assignment).WithMany(p => p.Rubrics)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rubric_Assignment");
        });

        modelBuilder.Entity<RubricCriterion>(entity =>
        {
            entity.HasKey(e => e.CriterionId).HasName("PK__RubricCr__647C3BB1DC0DB822");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);

            entity.HasOne(d => d.Rubric).WithMany(p => p.RubricCriteria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RubricCriterion_Rubric");
        });

        modelBuilder.Entity<RubricScore>(entity =>
        {
            entity.HasKey(e => e.RubricScoreId).HasName("PK__RubricSc__125909777F5DAB39");

            entity.HasOne(d => d.Criterion).WithMany(p => p.RubricScores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RubricScore_Criterion");

            entity.HasOne(d => d.Result).WithMany(p => p.RubricScores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RubricScore_Result");
        });

        modelBuilder.Entity<SubmissionAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__Submissi__442C64BEB8A44F51");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Submission).WithMany(p => p.SubmissionAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmissionAttachment_Submission");
        });

        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__SystemCo__C3BC335C4E59A0BA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C6940D45D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
