using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly EduNexusContext _context;

        public QuestionService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionListViewModel>> GetQuestionsByBankAsync(long bankId)
        {
            return await _context.Questions
                .Where(q => q.BankId == bankId)
                .Select(q => new QuestionListViewModel
                {
                    QuestionId = q.QuestionId,
                    BankId = q.BankId,
                    QuestionContent = q.QuestionContent,
                    QuestionType = q.QuestionType,
                    Difficulty = q.Difficulty,
                    IsAigenerated = q.IsAigenerated,
                    CreatedAt = q.CreatedAt,
                    ChoiceCount = q.Choices.Count
                })
                .ToListAsync();
        }

        public async Task<QuestionDetailViewModel?> GetQuestionDetailAsync(long questionId)
        {
            var question = await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);

            if (question == null) return null;

            return new QuestionDetailViewModel
            {
                QuestionId = question.QuestionId,
                QuestionContent = question.QuestionContent,
                QuestionType = question.QuestionType,
                Difficulty = question.Difficulty,
                Explanation = question.Explanation,
                Choices = question.Choices.OrderBy(c => c.DisplayOrder).Select(c => new ChoiceViewModel
                {
                    ChoiceId = c.ChoiceId,
                    ChoiceContent = c.ChoiceContent,
                    IsCorrect = c.IsCorrect,
                    DisplayOrder = c.DisplayOrder
                }).ToList()
            };
        }

        public async Task<QuestionFormViewModel?> GetQuestionForEditAsync(long questionId)
        {
            var question = await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);

            if (question == null) return null;

            return new QuestionFormViewModel
            {
                QuestionId = question.QuestionId,
                BankId = question.BankId,
                QuestionContent = question.QuestionContent,
                QuestionType = question.QuestionType,
                Difficulty = question.Difficulty,
                Explanation = question.Explanation,
                IsAigenerated = question.IsAigenerated ?? false,
                Choices = question.Choices.OrderBy(c => c.DisplayOrder).Select(c => new ChoiceFormViewModel
                {
                    ChoiceId = c.ChoiceId,
                    QuestionId = c.QuestionId,
                    ChoiceContent = c.ChoiceContent,
                    IsCorrect = c.IsCorrect ?? false,
                    DisplayOrder = c.DisplayOrder
                }).ToList()
            };
        }

        public async Task<long> CreateQuestionAsync(QuestionFormViewModel model, long smeId)
        {
            var question = new Question
            {
                BankId = model.BankId,
                QuestionContent = model.QuestionContent,
                QuestionType = model.QuestionType,
                Difficulty = model.Difficulty,
                Explanation = model.Explanation,
                CreatedBy = smeId,
                CreatedAt = DateTime.Now,
                IsAigenerated = model.IsAigenerated
            };

            foreach (var c in model.Choices)
            {
                question.Choices.Add(new Choice
                {
                    ChoiceContent = c.ChoiceContent,
                    IsCorrect = c.IsCorrect,
                    DisplayOrder = c.DisplayOrder
                });
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question.QuestionId;
        }

        public async Task<bool> UpdateQuestionAsync(QuestionFormViewModel model)
        {
            var question = await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.QuestionId == model.QuestionId);

            if (question == null) return false;

            question.QuestionContent = model.QuestionContent;
            question.QuestionType = model.QuestionType;
            question.Difficulty = model.Difficulty;
            question.Explanation = model.Explanation;
            question.IsAigenerated = model.IsAigenerated;

            var existingChoices = question.Choices.ToList();
            foreach (var modelChoice in model.Choices)
            {
                if (modelChoice.ChoiceId > 0)
                {
                    var existing = existingChoices.FirstOrDefault(c => c.ChoiceId == modelChoice.ChoiceId);
                    if (existing != null)
                    {
                        existing.ChoiceContent = modelChoice.ChoiceContent;
                        existing.IsCorrect = modelChoice.IsCorrect;
                        existing.DisplayOrder = modelChoice.DisplayOrder;
                    }
                }
                else
                {
                    question.Choices.Add(new Choice
                    {
                        ChoiceContent = modelChoice.ChoiceContent,
                        IsCorrect = modelChoice.IsCorrect,
                        DisplayOrder = modelChoice.DisplayOrder
                    });
                }
            }

            var modelChoiceIds = model.Choices.Where(c => c.ChoiceId > 0).Select(c => c.ChoiceId).ToList();
            var choicesToRemove = existingChoices.Where(c => !modelChoiceIds.Contains(c.ChoiceId)).ToList();
            _context.Choices.RemoveRange(choicesToRemove);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteQuestionAsync(long questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null) return false;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
