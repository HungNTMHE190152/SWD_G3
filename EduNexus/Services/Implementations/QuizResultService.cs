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
    public class QuizResultService : IQuizResultService
    {
        private readonly EduNexusContext _context;

        public QuizResultService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<QuizHistoryViewModel> GetStudentQuizHistoryAsync(long quizId, long studentId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            var attempts = await _context.QuizAttempts
                .Include(a => a.QuizAnswers)
                .Where(a => a.QuizId == quizId && a.StudentId == studentId)
                .OrderByDescending(a => a.AttemptNumber)
                .ToListAsync();

            var totalQuestions = await _context.QuizQuestions.CountAsync(qq => qq.QuizId == quizId);

            var vm = new QuizHistoryViewModel
            {
                QuizId = quizId,
                QuizTitle = quiz?.QuizTitle ?? "Unknown Quiz",
                Attempts = attempts.Select(a => new QuizResultViewModel
                {
                    AttemptId = a.AttemptId,
                    QuizId = a.QuizId,
                    QuizTitle = quiz?.QuizTitle ?? "",
                    StartTime = a.StartTime,
                    SubmitTime = a.SubmitTime,
                    TotalScore = a.TotalScore,
                    PassingScore = quiz?.PassingScore,
                    Status = a.Status,
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = a.QuizAnswers.Count(qa => qa.IsCorrect == true)
                }).ToList()
            };

            return vm;
        }

        public async Task<QuizResultViewModel?> GetAttemptResultAsync(long attemptId, long studentId)
        {
            var attempt = await _context.QuizAttempts
                .Include(a => a.Quiz)
                .Include(a => a.QuizAnswers)
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.StudentId == studentId);

            if (attempt == null) return null;

            var totalQuestions = await _context.QuizQuestions.CountAsync(qq => qq.QuizId == attempt.QuizId);

            return new QuizResultViewModel
            {
                AttemptId = attempt.AttemptId,
                QuizId = attempt.QuizId,
                QuizTitle = attempt.Quiz.QuizTitle,
                StartTime = attempt.StartTime,
                SubmitTime = attempt.SubmitTime,
                TotalScore = attempt.TotalScore,
                PassingScore = attempt.Quiz.PassingScore,
                Status = attempt.Status,
                TotalQuestions = totalQuestions,
                CorrectAnswers = attempt.QuizAnswers.Count(qa => qa.IsCorrect == true)
            };
        }

        public async Task<QuizReviewViewModel?> GetAttemptReviewAsync(long attemptId, long studentId)
        {
            var attempt = await _context.QuizAttempts
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.QuizQuestions)
                        .ThenInclude(qq => qq.Question)
                            .ThenInclude(q => q.Choices)
                .Include(a => a.QuizAnswers)
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.StudentId == studentId);

            if (attempt == null) return null;

            var vm = new QuizReviewViewModel
            {
                AttemptId = attempt.AttemptId,
                QuizTitle = attempt.Quiz.QuizTitle,
                TotalScore = attempt.TotalScore,
                Questions = new List<QuizReviewQuestionViewModel>()
            };

            foreach (var qq in attempt.Quiz.QuizQuestions)
            {
                var question = qq.Question;
                var answer = attempt.QuizAnswers.FirstOrDefault(qa => qa.QuestionId == question.QuestionId);

                var qvm = new QuizReviewQuestionViewModel
                {
                    QuestionId = question.QuestionId,
                    QuestionContent = question.QuestionContent,
                    QuestionType = question.QuestionType,
                    Explanation = question.Explanation,
                    SelectedChoiceId = answer?.ChoiceId,
                    AnswerText = answer?.AnswerText,
                    IsCorrect = answer?.IsCorrect,
                    Score = answer?.Score,
                    Choices = question.Choices.Select(c => new QuizReviewChoiceViewModel
                    {
                        ChoiceId = c.ChoiceId,
                        ChoiceContent = c.ChoiceContent,
                        IsCorrect = c.IsCorrect ?? false,
                        IsSelected = answer?.ChoiceId == c.ChoiceId
                    }).ToList()
                };

                vm.Questions.Add(qvm);
            }

            return vm;
        }
    }
}
