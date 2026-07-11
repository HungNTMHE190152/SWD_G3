using System.Threading.Tasks;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;

namespace EduNexus.Services.Implementations
{
    public class ChoiceService : IChoiceService
    {
        private readonly EduNexusContext _context;

        public ChoiceService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<ChoiceFormViewModel?> GetChoiceByIdAsync(long choiceId)
        {
            var choice = await _context.Choices.FindAsync(choiceId);
            if (choice == null) return null;

            return new ChoiceFormViewModel
            {
                ChoiceId = choice.ChoiceId,
                QuestionId = choice.QuestionId,
                ChoiceContent = choice.ChoiceContent,
                IsCorrect = choice.IsCorrect ?? false,
                DisplayOrder = choice.DisplayOrder
            };
        }

        public async Task<long> CreateChoiceAsync(ChoiceFormViewModel model)
        {
            var choice = new Choice
            {
                QuestionId = model.QuestionId,
                ChoiceContent = model.ChoiceContent,
                IsCorrect = model.IsCorrect,
                DisplayOrder = model.DisplayOrder
            };

            _context.Choices.Add(choice);
            await _context.SaveChangesAsync();
            return choice.ChoiceId;
        }

        public async Task<bool> UpdateChoiceAsync(ChoiceFormViewModel model)
        {
            var choice = await _context.Choices.FindAsync(model.ChoiceId);
            if (choice == null) return false;

            choice.ChoiceContent = model.ChoiceContent;
            choice.IsCorrect = model.IsCorrect;
            choice.DisplayOrder = model.DisplayOrder;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteChoiceAsync(long choiceId)
        {
            var choice = await _context.Choices.FindAsync(choiceId);
            if (choice == null) return false;

            _context.Choices.Remove(choice);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
