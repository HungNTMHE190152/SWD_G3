using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Quizzes;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class QuizAttemptService : IQuizAttemptService
{
    private readonly EduNexusContext _context;

    public QuizAttemptService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<NewQuizViewModel> GetAvailableQuizzesAsync(long studentId)
    {
        var quizzes = await (
            from quiz in _context.Quizzes
            join assignment in _context.Assignments
                on quiz.AssignmentId equals assignment.AssignmentId
            join module in _context.Modules
                on assignment.ModuleId equals module.ModuleId
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where assignment.Status == "PUBLISHED"
                  || assignment.Status == "Published"
            orderby quiz.CreatedAt descending
            select new
            {
                quiz.QuizId,
                quiz.QuizTitle,
                quiz.Description,
                quiz.Duration,
                quiz.PassingScore,
                CourseId = course.CourseId,
                CourseTitle = course.Title,
                ModuleName = module.ModuleName,
                QuestionCount = _context.QuizQuestions.Count(qq => qq.QuizId == quiz.QuizId),
                AttemptCount = _context.QuizAttempts.Count(a => a.QuizId == quiz.QuizId && a.StudentId == studentId),
                LastScore = _context.QuizAttempts
                    .Where(a => a.QuizId == quiz.QuizId && a.StudentId == studentId)
                    .OrderByDescending(a => a.AttemptNumber)
                    .Select(a => a.TotalScore)
                    .FirstOrDefault()
            }
        ).ToListAsync();

        var model = new NewQuizViewModel();

        foreach (var quiz in quizzes)
        {
            if (quiz.QuestionCount <= 0)
            {
                continue;
            }

            bool canAccess = await IsStudentEnrolledInCourseAsync(studentId, quiz.CourseId);

            if (!canAccess)
            {
                continue;
            }

            model.Quizzes.Add(new AvailableQuizItemViewModel
            {
                QuizId = quiz.QuizId,
                QuizTitle = quiz.QuizTitle,
                Description = quiz.Description,
                Duration = quiz.Duration,
                PassingScore = quiz.PassingScore,
                CourseTitle = quiz.CourseTitle,
                ModuleName = quiz.ModuleName,
                QuestionCount = quiz.QuestionCount,
                AttemptCount = quiz.AttemptCount,
                LastScore = quiz.LastScore
            });
        }

        return model;
    }

    public async Task<long> StartAttemptAsync(long quizId, long studentId)
    {
        var quizInfo = await GetQuizCourseInfoAsync(quizId);

        if (quizInfo == null)
        {
            throw new InvalidOperationException("Quiz not found.");
        }

        bool canAccess = await IsStudentEnrolledInCourseAsync(studentId, quizInfo.CourseId);

        if (!canAccess)
        {
            throw new InvalidOperationException("You are not enrolled in this course.");
        }

        int questionCount = await _context.QuizQuestions
            .CountAsync(qq => qq.QuizId == quizId);

        if (questionCount <= 0)
        {
            throw new InvalidOperationException("This quiz does not have questions yet.");
        }

        int nextAttemptNumber = await _context.QuizAttempts
            .Where(a => a.QuizId == quizId && a.StudentId == studentId)
            .Select(a => (int?)a.AttemptNumber)
            .MaxAsync() ?? 0;

        nextAttemptNumber++;

        var attempt = new QuizAttempt
        {
            QuizId = quizId,
            StudentId = studentId,
            AttemptNumber = nextAttemptNumber,
            StartTime = DateTime.Now,
            Status = "IN_PROGRESS"
        };

        _context.QuizAttempts.Add(attempt);
        await _context.SaveChangesAsync();

        return attempt.AttemptId;
    }

    public async Task<QuizTakingViewModel?> GetTakingAsync(long attemptId, long studentId)
    {
        var attemptInfo = await (
            from attempt in _context.QuizAttempts
            join quiz in _context.Quizzes
                on attempt.QuizId equals quiz.QuizId
            where attempt.AttemptId == attemptId
                  && attempt.StudentId == studentId
            select new
            {
                attempt.AttemptId,
                attempt.QuizId,
                attempt.StartTime,
                attempt.Status,
                quiz.QuizTitle,
                quiz.Description,
                quiz.Duration,
                quiz.ShuffleQuestion,
                quiz.ShuffleAnswer
            }
        ).FirstOrDefaultAsync();

        if (attemptInfo == null)
        {
            return null;
        }

        if (attemptInfo.Status != "IN_PROGRESS")
        {
            return null;
        }

        var quizQuestions = await (
            from quizQuestion in _context.QuizQuestions
            join question in _context.Questions
                on quizQuestion.QuestionId equals question.QuestionId
            where quizQuestion.QuizId == attemptInfo.QuizId
            orderby quizQuestion.DisplayOrder
            select new
            {
                quizQuestion.DisplayOrder,
                question.QuestionId,
                question.QuestionContent,
                question.QuestionType
            }
        ).ToListAsync();

        if (attemptInfo.ShuffleQuestion == true)
        {
            quizQuestions = quizQuestions
                .OrderBy(_ => Guid.NewGuid())
                .ToList();
        }

        var model = new QuizTakingViewModel
        {
            AttemptId = attemptInfo.AttemptId,
            QuizId = attemptInfo.QuizId,
            QuizTitle = attemptInfo.QuizTitle,
            Description = attemptInfo.Description,
            Duration = attemptInfo.Duration,
            StartTime = attemptInfo.StartTime
        };

        int order = 1;

        foreach (var question in quizQuestions)
        {
            var choices = await _context.Choices
                .Where(c => c.QuestionId == question.QuestionId)
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new QuizTakingChoiceViewModel
                {
                    ChoiceId = c.ChoiceId,
                    ChoiceContent = c.ChoiceContent,
                    DisplayOrder = c.DisplayOrder ?? 1
                })
                .ToListAsync();

            if (attemptInfo.ShuffleAnswer == true)
            {
                choices = choices
                    .OrderBy(_ => Guid.NewGuid())
                    .ToList();
            }

            model.Questions.Add(new QuizTakingQuestionViewModel
            {
                QuestionId = question.QuestionId,
                QuestionContent = question.QuestionContent,
                QuestionType = question.QuestionType,
                DisplayOrder = order,
                Choices = choices
            });

            order++;
        }

        return model;
    }

    public async Task<long> SubmitAsync(QuizSubmitViewModel model, long studentId)
    {
        var attempt = await _context.QuizAttempts
            .FirstOrDefaultAsync(a => a.AttemptId == model.AttemptId && a.StudentId == studentId);

        if (attempt == null)
        {
            throw new InvalidOperationException("Attempt not found.");
        }

        if (attempt.Status != "IN_PROGRESS")
        {
            return attempt.AttemptId;
        }

        var quizQuestions = await _context.QuizQuestions
            .Where(qq => qq.QuizId == attempt.QuizId)
            .OrderBy(qq => qq.DisplayOrder)
            .ToListAsync();

        if (!quizQuestions.Any())
        {
            throw new InvalidOperationException("This quiz does not have questions.");
        }

        var oldAnswers = await _context.QuizAnswers
            .Where(a => a.AttemptId == attempt.AttemptId)
            .ToListAsync();

        _context.QuizAnswers.RemoveRange(oldAnswers);
        await _context.SaveChangesAsync();

        decimal maxScore = quizQuestions.Sum(q => q.Score ?? 1);
        decimal earnedScore = 0;

        foreach (var quizQuestion in quizQuestions)
        {
            var submittedAnswer = model.Answers
                .FirstOrDefault(a => a.QuestionId == quizQuestion.QuestionId);

            long? selectedChoiceId = submittedAnswer?.ChoiceId;

            Choice? selectedChoice = null;

            if (selectedChoiceId != null)
            {
                selectedChoice = await _context.Choices
                    .FirstOrDefaultAsync(c =>
                        c.ChoiceId == selectedChoiceId.Value &&
                        c.QuestionId == quizQuestion.QuestionId);
            }

            bool isCorrect = selectedChoice?.IsCorrect == true;
            decimal questionScore = quizQuestion.Score ?? 1;
            decimal finalScore = isCorrect ? questionScore : 0;

            earnedScore += finalScore;

            var answer = new QuizAnswer
            {
                AttemptId = attempt.AttemptId,
                QuestionId = quizQuestion.QuestionId,
                ChoiceId = selectedChoice?.ChoiceId,
                IsCorrect = isCorrect,
                Score = finalScore
            };

            _context.QuizAnswers.Add(answer);
        }

        decimal totalScore = maxScore <= 0
            ? 0
            : Math.Round(earnedScore / maxScore * 10, 2);

        attempt.SubmitTime = DateTime.Now;
        attempt.TotalScore = totalScore;
        attempt.Status = "GRADED";

        await _context.SaveChangesAsync();

        return attempt.AttemptId;
    }

    public async Task<QuizResultViewModel?> GetResultAsync(long attemptId, long studentId)
    {
        var result = await (
            from attempt in _context.QuizAttempts
            join quiz in _context.Quizzes
                on attempt.QuizId equals quiz.QuizId
            join assignment in _context.Assignments
                on quiz.AssignmentId equals assignment.AssignmentId
            join module in _context.Modules
                on assignment.ModuleId equals module.ModuleId
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where attempt.AttemptId == attemptId
                  && attempt.StudentId == studentId
            select new QuizResultViewModel
            {
                AttemptId = attempt.AttemptId,
                QuizId = quiz.QuizId,
                QuizTitle = quiz.QuizTitle,
                CourseTitle = course.Title,
                ModuleName = module.ModuleName,
                StartTime = attempt.StartTime,
                SubmitTime = attempt.SubmitTime,
                TotalScore = attempt.TotalScore,
                PassingScore = quiz.PassingScore,
                Status = attempt.Status
            }
        ).FirstOrDefaultAsync();

        if (result == null)
        {
            return null;
        }

        result.TotalQuestions = await _context.QuizAnswers
            .CountAsync(a => a.AttemptId == attemptId);

        result.CorrectAnswers = await _context.QuizAnswers
            .CountAsync(a => a.AttemptId == attemptId && a.IsCorrect == true);

        result.WrongAnswers = result.TotalQuestions - result.CorrectAnswers;

        result.IsPassed = result.TotalScore >= (result.PassingScore ?? 0);

        return result;
    }

    public async Task<QuizHistoryViewModel> GetHistoryAsync(long studentId)
    {
        var attempts = await (
            from attempt in _context.QuizAttempts
            join quiz in _context.Quizzes
                on attempt.QuizId equals quiz.QuizId
            join assignment in _context.Assignments
                on quiz.AssignmentId equals assignment.AssignmentId
            join module in _context.Modules
                on assignment.ModuleId equals module.ModuleId
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where attempt.StudentId == studentId
            orderby attempt.StartTime descending
            select new QuizHistoryItemViewModel
            {
                AttemptId = attempt.AttemptId,
                QuizId = quiz.QuizId,
                QuizTitle = quiz.QuizTitle,
                CourseTitle = course.Title,
                AttemptNumber = attempt.AttemptNumber ?? 1,
                StartTime = attempt.StartTime,
                SubmitTime = attempt.SubmitTime,
                TotalScore = attempt.TotalScore,
                PassingScore = quiz.PassingScore,
                Status = attempt.Status,
                IsPassed = attempt.TotalScore >= (quiz.PassingScore ?? 0)
            }
        ).ToListAsync();

        return new QuizHistoryViewModel
        {
            Attempts = attempts
        };
    }

    public async Task<QuizReviewViewModel?> GetReviewAsync(long attemptId, long studentId)
    {
        var attemptInfo = await (
            from attempt in _context.QuizAttempts
            join quiz in _context.Quizzes
                on attempt.QuizId equals quiz.QuizId
            where attempt.AttemptId == attemptId
                  && attempt.StudentId == studentId
            select new
            {
                attempt.AttemptId,
                attempt.TotalScore,
                quiz.QuizTitle
            }
        ).FirstOrDefaultAsync();

        if (attemptInfo == null)
        {
            return null;
        }

        var model = new QuizReviewViewModel
        {
            AttemptId = attemptInfo.AttemptId,
            QuizTitle = attemptInfo.QuizTitle,
            TotalScore = attemptInfo.TotalScore
        };

        var reviewItems = await (
            from answer in _context.QuizAnswers
            join question in _context.Questions
                on answer.QuestionId equals question.QuestionId
            where answer.AttemptId == attemptId
            orderby question.QuestionId
            select new
            {
                question.QuestionId,
                question.QuestionContent,
                question.Explanation,
                answer.ChoiceId,
                answer.IsCorrect,
                answer.Score
            }
        ).ToListAsync();

        foreach (var item in reviewItems)
        {
            string? selectedAnswer = null;

            if (item.ChoiceId != null)
            {
                selectedAnswer = await _context.Choices
                    .Where(c => c.ChoiceId == item.ChoiceId.Value)
                    .Select(c => c.ChoiceContent)
                    .FirstOrDefaultAsync();
            }

            string? correctAnswer = await _context.Choices
                .Where(c => c.QuestionId == item.QuestionId && c.IsCorrect == true)
                .OrderBy(c => c.DisplayOrder)
                .Select(c => c.ChoiceContent)
                .FirstOrDefaultAsync();

            model.Questions.Add(new QuizReviewQuestionViewModel
            {
                QuestionId = item.QuestionId,
                QuestionContent = item.QuestionContent,
                Explanation = item.Explanation,
                SelectedAnswer = selectedAnswer ?? "No answer",
                CorrectAnswer = correctAnswer,
                IsCorrect = item.IsCorrect == true,
                Score = item.Score
            });
        }

        return model;
    }

    private async Task<bool> IsStudentEnrolledInCourseAsync(long studentId, long courseId)
    {
        return await _context.Enrollments
            .AnyAsync(e =>
                e.StudentId == studentId &&
                e.CourseId == courseId &&
                (
                    e.Status == "ACTIVE" ||
                    e.Status == "Active" ||
                    e.Status == "COMPLETED" ||
                    e.Status == "Completed"
                ));
    }

    private async Task<QuizCourseInfo?> GetQuizCourseInfoAsync(long quizId)
    {
        return await (
            from quiz in _context.Quizzes
            join assignment in _context.Assignments
                on quiz.AssignmentId equals assignment.AssignmentId
            join module in _context.Modules
                on assignment.ModuleId equals module.ModuleId
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where quiz.QuizId == quizId
            select new QuizCourseInfo
            {
                QuizId = quiz.QuizId,
                CourseId = course.CourseId
            }
        ).FirstOrDefaultAsync();
    }

    private class QuizCourseInfo
    {
        public long QuizId { get; set; }
        public long CourseId { get; set; }
    }
}