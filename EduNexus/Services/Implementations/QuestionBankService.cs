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
    public class QuestionBankService : IQuestionBankService
    {
        private readonly EduNexusContext _context;

        public QuestionBankService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionBankListViewModel>> GetBanksByCourseAsync(long courseId)
        {
            return await _context.QuestionBanks
                .Where(b => b.CourseId == courseId)
                .Select(b => new QuestionBankListViewModel
                {
                    BankId = b.BankId,
                    BankName = b.BankName,
                    Description = b.Description,
                    IsPublic = b.IsPublic,
                    CreatedAt = b.CreatedAt,
                    QuestionCount = b.Questions.Count
                })
                .ToListAsync();
        }

        public async Task<QuestionBankFormViewModel?> GetBankByIdAsync(long id)
        {
            var bank = await _context.QuestionBanks.FindAsync(id);
            if (bank == null) return null;

            return new QuestionBankFormViewModel
            {
                BankId = bank.BankId,
                CourseId = bank.CourseId,
                BankName = bank.BankName,
                Description = bank.Description,
                IsPublic = bank.IsPublic ?? false
            };
        }

        public async Task<long> CreateBankAsync(QuestionBankFormViewModel model, long smeId)
        {
            var bank = new QuestionBank
            {
                CourseId = model.CourseId,
                BankName = model.BankName,
                Description = model.Description,
                IsPublic = model.IsPublic,
                CreatedBy = smeId,
                CreatedAt = DateTime.Now
            };

            _context.QuestionBanks.Add(bank);
            await _context.SaveChangesAsync();
            return bank.BankId;
        }

        public async Task<bool> UpdateBankAsync(QuestionBankFormViewModel model)
        {
            var bank = await _context.QuestionBanks.FindAsync(model.BankId);
            if (bank == null) return false;

            bank.BankName = model.BankName;
            bank.Description = model.Description;
            bank.IsPublic = model.IsPublic;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBankAsync(long id)
        {
            var bank = await _context.QuestionBanks.FindAsync(id);
            if (bank == null) return false;

            _context.QuestionBanks.Remove(bank);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
