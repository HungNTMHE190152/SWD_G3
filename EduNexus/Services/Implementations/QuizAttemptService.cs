using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Quizzes;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations
{
    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly EduNexusContext _context;

        public QuizAttemptService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<NewQuizViewModel>> GetAvailableQuizzesForStudentAsync(long studentId)
        {
            var enrolledCourseIds = await _context.Enrollments
                .Where(e => e.StudentId == studentId && e.Status == "Active")
                .Select(e => e.CourseId)
                .ToListAsync();

            var quizzes = await _context.Quizzes
                .Include(q => q.Assignment)
                    .ThenInclude(a => a.Module)
                .Include(q => q.QuizQuestions)
                .Include(q => q.QuizAttempts.Where(qa => qa.StudentId == studentId))
                .Where(q => enrolledCourseIds.Contains(q.Assignment.Module.CourseId))
                .ToListAsync();

            return quizzes.Select(q => new NewQuizViewModel
            {
                QuizId = q.QuizId,
                QuizTitle = q.QuizTitle,
                Description = q.Description,
                Duration = q.Duration,
                PassingScore = q.PassingScore,
                QuestionCount = q.QuizQuestions.Count,
                HasPreviousAttempts = q.QuizAttempts.Any(),
                PreviousAttemptCount = q.QuizAttempts.Count
            }).ToList();
        }

        public async Task<NewQuizViewModel?> GetQuizDetailsForStudentAsync(long quizId, long studentId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .Include(q => q.QuizAttempts.Where(qa => qa.StudentId == studentId))
                .FirstOrDefaultAsync(q => q.QuizId == quizId);

            if (quiz == null) return null;

            return new NewQuizViewModel
            {
                QuizId = quiz.QuizId,
                QuizTitle = quiz.QuizTitle,
                Description = quiz.Description,
                Duration = quiz.Duration,
                PassingScore = quiz.PassingScore,
                QuestionCount = quiz.QuizQuestions.Count,
                HasPreviousAttempts = quiz.QuizAttempts.Any(),
                PreviousAttemptCount = quiz.QuizAttempts.Count
            };
        }

        public async Task<QuizTakingViewModel> StartQuizAsync(long quizId, long studentId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                    .ThenInclude(qq => qq.Question)
                        .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);

            if (quiz == null) throw new Exception("Quiz not found");

            var attemptCount = await _context.QuizAttempts
                .Where(qa => qa.QuizId == quizId && qa.StudentId == studentId)
                .CountAsync();

            var newAttempt = new QuizAttempt
            {
                QuizId = quizId,
                StudentId = studentId,
                AttemptNumber = attemptCount + 1,
                StartTime = DateTime.Now,
                Status = "InProgress"
            };

            _context.QuizAttempts.Add(newAttempt);
            await _context.SaveChangesAsync();

            var vm = new QuizTakingViewModel
            {
                AttemptId = newAttempt.AttemptId,
                QuizId = quiz.QuizId,
                QuizTitle = quiz.QuizTitle,
                Duration = quiz.Duration,
                StartTime = newAttempt.StartTime,
                Questions = quiz.QuizQuestions.Select(qq => new QuizTakingQuestionViewModel
                {
                    QuestionId = qq.Question.QuestionId,
                    QuestionContent = qq.Question.QuestionContent,
                    QuestionType = qq.Question.QuestionType,
                    Choices = qq.Question.Choices.OrderBy(c => c.DisplayOrder).Select(c => new QuizTakingChoiceViewModel
                    {
                        ChoiceId = c.ChoiceId,
                        ChoiceContent = c.ChoiceContent
                    }).ToList()
                }).ToList()
            };

            if (quiz.ShuffleQuestion == true)
            {
                var rng = new Random();
                vm.Questions = vm.Questions.OrderBy(x => rng.Next()).ToList();
            }

            if (quiz.ShuffleAnswer == true)
            {
                var rng = new Random();
                foreach (var q in vm.Questions)
                {
                    q.Choices = q.Choices.OrderBy(x => rng.Next()).ToList();
                }
            }

            return vm;
        }

        public async Task<QuizTakingViewModel?> GetQuizTakingModelAsync(long attemptId, long studentId)
        {
            var attempt = await _context.QuizAttempts
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.QuizQuestions)
                        .ThenInclude(qq => qq.Question)
                            .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.StudentId == studentId);

            if (attempt == null) return null;

            return new QuizTakingViewModel
            {
                AttemptId = attempt.AttemptId,
                QuizId = attempt.QuizId,
                QuizTitle = attempt.Quiz.QuizTitle,
                Duration = attempt.Quiz.Duration,
                StartTime = attempt.StartTime,
                Questions = attempt.Quiz.QuizQuestions.Select(qq => new QuizTakingQuestionViewModel
                {
                    QuestionId = qq.Question.QuestionId,
                    QuestionContent = qq.Question.QuestionContent,
                    QuestionType = qq.Question.QuestionType,
                    Choices = qq.Question.Choices.OrderBy(c => c.DisplayOrder).Select(c => new QuizTakingChoiceViewModel
                    {
                        ChoiceId = c.ChoiceId,
                        ChoiceContent = c.ChoiceContent
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<bool> SubmitQuizAsync(QuizSubmitViewModel model, long studentId)
        {
            var attempt = await _context.QuizAttempts
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.QuizQuestions)
                        .ThenInclude(qq => qq.Question)
                            .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(a => a.AttemptId == model.AttemptId && a.StudentId == studentId);

            if (attempt == null || attempt.Status == "Completed") return false;

            decimal totalScore = 0;

            foreach (var qq in attempt.Quiz.QuizQuestions)
            {
                var question = qq.Question;
                long? selectedChoiceId = model.SelectedChoices.ContainsKey(question.QuestionId) ? model.SelectedChoices[question.QuestionId] : null;
                string? textAnswer = model.TextAnswers.ContainsKey(question.QuestionId) ? model.TextAnswers[question.QuestionId] : null;

                bool isCorrect = false;
                decimal score = 0;

                if (question.QuestionType == "Single Choice" && selectedChoiceId.HasValue)
                {
                    var correctChoice = question.Choices.FirstOrDefault(c => c.IsCorrect == true);
                    if (correctChoice != null && correctChoice.ChoiceId == selectedChoiceId.Value)
                    {
                        isCorrect = true;
                        score = 1;
                    }
                }
                else if (question.QuestionType == "True/False" && selectedChoiceId.HasValue)
                {
                    var correctChoice = question.Choices.FirstOrDefault(c => c.IsCorrect == true);
                    if (correctChoice != null && correctChoice.ChoiceId == selectedChoiceId.Value)
                    {
                        isCorrect = true;
                        score = 1;
                    }
                }
                else if (question.QuestionType == "Short Answer")
                {
                    var correctChoice = question.Choices.FirstOrDefault(c => c.IsCorrect == true);
                    if (correctChoice != null && !string.IsNullOrWhiteSpace(textAnswer) && correctChoice.ChoiceContent.Equals(textAnswer.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        isCorrect = true;
                        score = 1;
                    }
                }

                totalScore += score;

                _context.QuizAnswers.Add(new QuizAnswer
                {
                    AttemptId = attempt.AttemptId,
                    QuestionId = question.QuestionId,
                    ChoiceId = selectedChoiceId,
                    AnswerText = textAnswer,
                    IsCorrect = isCorrect,
                    Score = score
                });
            }

            attempt.SubmitTime = DateTime.Now;
            attempt.TotalScore = totalScore;
            attempt.Status = "Completed";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
