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
    public class QuizService : IQuizService
    {
        private readonly EduNexusContext _context;

        public QuizService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<QuizListViewModel>> GetQuizzesByAssignmentAsync(long assignmentId)
        {
            return await _context.Quizzes
                .Where(q => q.AssignmentId == assignmentId)
                .Select(q => new QuizListViewModel
                {
                    QuizId = q.QuizId,
                    QuizTitle = q.QuizTitle,
                    Duration = q.Duration,
                    PassingScore = q.PassingScore,
                    CreatedAt = q.CreatedAt,
                    QuestionCount = q.QuizQuestions.Count
                })
                .ToListAsync();
        }

        public async Task<QuizFormViewModel?> GetQuizByIdAsync(long quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null) return null;

            return new QuizFormViewModel
            {
                QuizId = quiz.QuizId,
                AssignmentId = quiz.AssignmentId,
                QuizTitle = quiz.QuizTitle,
                Description = quiz.Description,
                Duration = quiz.Duration,
                PassingScore = quiz.PassingScore,
                ShuffleQuestion = quiz.ShuffleQuestion ?? false,
                ShuffleAnswer = quiz.ShuffleAnswer ?? false
            };
        }

        public async Task<long> CreateQuizAsync(QuizFormViewModel model, long smeId)
        {
            var quiz = new Quiz
            {
                AssignmentId = model.AssignmentId,
                QuizTitle = model.QuizTitle,
                Description = model.Description,
                Duration = model.Duration,
                PassingScore = model.PassingScore,
                ShuffleQuestion = model.ShuffleQuestion,
                ShuffleAnswer = model.ShuffleAnswer,
                CreatedBy = smeId,
                CreatedAt = DateTime.Now
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz.QuizId;
        }

        public async Task<bool> UpdateQuizAsync(QuizFormViewModel model)
        {
            var quiz = await _context.Quizzes.FindAsync(model.QuizId);
            if (quiz == null) return false;

            quiz.AssignmentId = model.AssignmentId;
            quiz.QuizTitle = model.QuizTitle;
            quiz.Description = model.Description;
            quiz.Duration = model.Duration;
            quiz.PassingScore = model.PassingScore;
            quiz.ShuffleQuestion = model.ShuffleQuestion;
            quiz.ShuffleAnswer = model.ShuffleAnswer;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteQuizAsync(long quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null) return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<QuizQuestionSelectionViewModel> GetQuizQuestionSelectionAsync(long quizId, long bankId)
        {
            var bankQuestions = await _context.Questions
                .Where(q => q.BankId == bankId)
                .ToListAsync();

            var existingQuizQuestionIds = await _context.QuizQuestions
                .Where(qq => qq.QuizId == quizId)
                .Select(qq => qq.QuestionId)
                .ToListAsync();

            var vm = new QuizQuestionSelectionViewModel
            {
                QuizId = quizId,
                BankId = bankId,
                AvailableQuestions = bankQuestions.Select(q => new QuizQuestionItemViewModel
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.QuestionContent,
                    QuestionType = q.QuestionType,
                    IsAlreadyInQuiz = existingQuizQuestionIds.Contains(q.QuestionId)
                }).ToList()
            };

            return vm;
        }

        public async Task<bool> AddQuestionsToQuizAsync(long quizId, List<long> questionIds)
        {
            var existingQuizQuestionIds = await _context.QuizQuestions
                .Where(qq => qq.QuizId == quizId)
                .Select(qq => qq.QuestionId)
                .ToListAsync();

            foreach (var qId in questionIds)
            {
                if (!existingQuizQuestionIds.Contains(qId))
                {
                    _context.QuizQuestions.Add(new QuizQuestion
                    {
                        QuizId = quizId,
                        QuestionId = qId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveQuestionFromQuizAsync(long quizId, long questionId)
        {
            var qq = await _context.QuizQuestions
                .FirstOrDefaultAsync(x => x.QuizId == quizId && x.QuestionId == questionId);
            
            if (qq != null)
            {
                _context.QuizQuestions.Remove(qq);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
