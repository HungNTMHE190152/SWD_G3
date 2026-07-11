// Services/RubricCriterionService.cs
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels;
using EduNexus.ViewModels.Rubric;
using EduNexus.ViewModels.RubricCriterion;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services
{
    public class RubricCriterionService : IRubricCriterionService
    {
        private readonly EduNexusContext _context;

        public RubricCriterionService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<RubricManagementViewModel> GetCriteriaAsync(long rubricId)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Assignment)
                .FirstOrDefaultAsync(r => r.RubricId == rubricId);

            if (rubric == null) return null!;

            var criteria = await _context.RubricCriteria
                .Where(c => c.RubricId == rubricId)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CriterionName)
                .Select(c => new RubricCriterionItemViewModel
                {
                    CriterionId = c.CriterionId,           // SỬA Ở ĐÂY
                    RubricId = c.RubricId,
                    CriterionName = c.CriterionName,
                    Description = c.Description,
                    Weight = c.Weight,
                    MaxScore = c.MaxScore,
                    DisplayOrder = c.DisplayOrder ?? 0
                })
                .ToListAsync();

            return new RubricManagementViewModel
            {
                RubricId = rubric.RubricId,
                RubricName = rubric.RubricName,
                AssignmentId = rubric.AssignmentId,
                AssignmentTitle = rubric.Assignment?.Title ?? "N/A",  // SỬA Ở ĐÂY
                TotalWeight = rubric.TotalWeight ?? 0,
                CurrentCriteria = criteria
            };
        }

        public async Task AddCriterionAsync(CreateCriterionViewModel model)
        {
            await ValidateCriterionAsync(model.RubricId, model.CriterionName, model.Weight);

            var criterion = new RubricCriterion
            {
                RubricId = model.RubricId,
                CriterionName = model.CriterionName,
                Description = model.Description,
                Weight = model.Weight,
                MaxScore = model.MaxScore,
                DisplayOrder = model.DisplayOrder
            };

            _context.RubricCriteria.Add(criterion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCriteriaAsync(List<RubricCriterionItemViewModel> criteria)
        {
            if (criteria == null || !criteria.Any()) return;

            long rubricId = criteria.First().RubricId;

            var rubric = await _context.Rubrics.FindAsync(rubricId);
            decimal totalWeight = criteria.Sum(c => c.Weight);

            if (rubric != null && totalWeight > (rubric.TotalWeight ?? 0))
            {
                throw new InvalidOperationException($"Tổng Weight ({totalWeight}%) không được vượt quá TotalWeight của Rubric ({rubric.TotalWeight}%).");
            }

            foreach (var item in criteria)
            {
                var criterion = await _context.RubricCriteria.FindAsync(item.CriterionId);  // SỬA Ở ĐÂY
                if (criterion != null)
                {
                    criterion.CriterionName = item.CriterionName;
                    criterion.Description = item.Description;
                    criterion.Weight = item.Weight;
                    criterion.MaxScore = item.MaxScore;
                    criterion.DisplayOrder = item.DisplayOrder;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCriterionAsync(long criterionId)   // SỬA THAM SỐ
        {
            var criterion = await _context.RubricCriteria.FindAsync(criterionId);
            if (criterion != null)
            {
                _context.RubricCriteria.Remove(criterion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<EditCriterionViewModel?> GetForEditAsync(long id)
        {
            var criterion = await _context.RubricCriteria.FindAsync(id);
            if (criterion == null) return null;

            return new EditCriterionViewModel
            {
                CriterionId = criterion.CriterionId,        // SỬA
                RubricId = criterion.RubricId,
                CriterionName = criterion.CriterionName,
                Description = criterion.Description,
                Weight = criterion.Weight,
                MaxScore = criterion.MaxScore,
                DisplayOrder = criterion.DisplayOrder ?? 0
            };
        }

        public async Task UpdateCriterionAsync(EditCriterionViewModel model)
        {
            await ValidateCriterionAsync(model.RubricId, model.CriterionName, model.Weight, model.CriterionId);

            var criterion = await _context.RubricCriteria.FindAsync(model.CriterionId);
            if (criterion == null) throw new InvalidOperationException("Không tìm thấy tiêu chí.");

            criterion.CriterionName = model.CriterionName;
            criterion.Description = model.Description;
            criterion.Weight = model.Weight;
            criterion.MaxScore = model.MaxScore;
            criterion.DisplayOrder = model.DisplayOrder;

            await _context.SaveChangesAsync();
        }

        private async Task ValidateCriterionAsync(long rubricId, string name, decimal weight, long? excludeId = null)
        {
            var exists = await _context.RubricCriteria
                .AnyAsync(c => c.RubricId == rubricId
                            && c.CriterionName == name
                            && c.CriterionId != excludeId);

            if (exists)
                throw new InvalidOperationException("Tên tiêu chí đã tồn tại trong Rubric này.");
        }
    }
}