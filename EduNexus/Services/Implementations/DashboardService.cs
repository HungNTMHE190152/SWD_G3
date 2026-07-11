using EduNexus.Data;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly EduNexusContext _context;

    public DashboardService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<StudentDashboardViewModel> GetStudentDashboardAsync(long studentId)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == studentId);

        var courseEnrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId && e.CourseId != null)
            .ToListAsync();

        var packageEnrollments = await _context.Enrollments
            .Include(e => e.CourseGroup)
            .Where(e => e.StudentId == studentId && e.CourseGroupId != null)
            .ToListAsync();

        var recentQuizAttempts = await _context.QuizAttempts
            .Include(a => a.Quiz)
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.StartTime)
            .Take(5)
            .Select(a => new RecentQuizAttemptViewModel
            {
                AttemptId = a.AttemptId,
                QuizTitle = a.Quiz.QuizTitle,
                TotalScore = a.TotalScore,
                Status = a.Status ?? string.Empty,
                StartTime = a.StartTime,
                SubmitTime = a.SubmitTime
            })
            .ToListAsync();

        var recentAssignmentResults = await _context.EssaySubmissions
            .Include(s => s.Assignment)
            .Include(s => s.EssayResult)
            .Where(s => s.StudentId == studentId)
            .OrderByDescending(s => s.SubmittedAt)
            .Take(5)
            .Select(s => new RecentAssignmentResultViewModel
            {
                SubmissionId = s.SubmissionId,
                AssignmentTitle = s.Assignment.Title,
                TotalScore = s.EssayResult != null ? s.EssayResult.TotalScore : null,
                GradingStatus = s.GradingStatus ?? string.Empty,
                SubmittedAt = s.SubmittedAt
            })
            .ToListAsync();

        int totalLessons = 0;
        int completedLessons = 0;

        foreach (var enrollment in courseEnrollments)
        {
            int courseLessons = await _context.Lessons
                .Include(l => l.Module)
                .Where(l => l.Module.CourseId == enrollment.CourseId && l.IsPublished == true)
                .CountAsync();

            int courseCompletedLessons = await _context.Progresses
                .Where(p => p.EnrollmentId == enrollment.EnrollmentId && p.IsCompleted == true)
                .CountAsync();

            totalLessons += courseLessons;
            completedLessons += courseCompletedLessons;
        }

        decimal overallProgress = courseEnrollments.Any()
            ? Math.Round(courseEnrollments.Average(e => e.Progress ?? 0), 2)
            : 0;

        return new StudentDashboardViewModel
        {
            StudentName = student?.FullName ?? "Student",
            TotalCourses = courseEnrollments.Count,
            TotalPackages = packageEnrollments.Count,
            CompletedLessons = completedLessons,
            TotalLessons = totalLessons,
            OverallProgress = overallProgress,

            Courses = courseEnrollments.Select(e => new StudentCourseItemViewModel
            {
                EnrollmentId = e.EnrollmentId,
                CourseId = e.CourseId ?? 0,
                CourseCode = e.Course?.CourseCode ?? string.Empty,
                CourseTitle = e.Course?.Title ?? string.Empty,
                ProgressPercentage = e.Progress ?? 0,
                Status = e.Status ?? string.Empty,
                EnrollDate = e.EnrollDate
            }).ToList(),

            Packages = packageEnrollments.Select(e => new StudentPackageItemViewModel
            {
                EnrollmentId = e.EnrollmentId,
                CourseGroupId = e.CourseGroupId ?? 0,
                GroupName = e.CourseGroup?.GroupName ?? string.Empty,
                Description = e.CourseGroup?.Description,
                ProgressPercentage = e.Progress ?? 0,
                EnrollDate = e.EnrollDate
            }).ToList(),

            RecentQuizAttempts = recentQuizAttempts,
            RecentAssignmentResults = recentAssignmentResults
        };
    }

    public async Task<PersonalProgressViewModel> GetPersonalProgressAsync(long studentId)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == studentId);

        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId && e.CourseId != null)
            .ToListAsync();

        var courseProgressList = new List<CourseProgressDetailViewModel>();

        foreach (var enrollment in enrollments)
        {
            int totalLessons = await _context.Lessons
                .Include(l => l.Module)
                .Where(l => l.Module.CourseId == enrollment.CourseId && l.IsPublished == true)
                .CountAsync();

            int completedLessons = await _context.Progresses
                .Where(p => p.EnrollmentId == enrollment.EnrollmentId && p.IsCompleted == true)
                .CountAsync();

            courseProgressList.Add(new CourseProgressDetailViewModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                CourseId = enrollment.CourseId ?? 0,
                CourseTitle = enrollment.Course?.Title ?? string.Empty,
                CompletedLessons = completedLessons,
                TotalLessons = totalLessons,
                ProgressPercentage = enrollment.Progress ?? 0
            });
        }

        var quizProgressList = await _context.QuizAttempts
            .Include(a => a.Quiz)
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.StartTime)
            .Select(a => new QuizProgressItemViewModel
            {
                AttemptId = a.AttemptId,
                QuizTitle = a.Quiz.QuizTitle,
                TotalScore = a.TotalScore,
                Status = a.Status ?? string.Empty,
                StartTime = a.StartTime
            })
            .ToListAsync();

        var assignmentProgressList = await _context.EssaySubmissions
            .Include(s => s.Assignment)
            .Include(s => s.EssayResult)
            .Where(s => s.StudentId == studentId)
            .OrderByDescending(s => s.SubmittedAt)
            .Select(s => new AssignmentProgressItemViewModel
            {
                SubmissionId = s.SubmissionId,
                AssignmentTitle = s.Assignment.Title,
                TotalScore = s.EssayResult != null ? s.EssayResult.TotalScore : null,
                GradingStatus = s.GradingStatus ?? string.Empty,
                SubmittedAt = s.SubmittedAt
            })
            .ToListAsync();

        decimal overallProgress = courseProgressList.Any()
            ? Math.Round(courseProgressList.Average(c => c.ProgressPercentage), 2)
            : 0;

        return new PersonalProgressViewModel
        {
            StudentId = studentId,
            StudentName = student?.FullName ?? "Student",
            OverallProgress = overallProgress,
            CourseProgressList = courseProgressList,
            QuizProgressList = quizProgressList,
            AssignmentProgressList = assignmentProgressList
        };
    }
}