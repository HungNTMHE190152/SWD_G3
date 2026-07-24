using EduNexus.Data;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;
using EduNexus.Models;

namespace EduNexus.Services.Assignment.Implementations
{
    public class RubricService : IRubricService
    {
        private readonly EduNexusContext _context;

        public RubricService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<RubricViewModel>> GetAllAsync(long smeId)
        {
            return await _context.Rubrics
                .Include(r => r.AssignmentTemplate)
                .Where(r => r.AssignmentTemplate.CreatedBy == smeId)
                .Select(r => new RubricViewModel
                {
                    RubricId = r.RubricId,
                    AssignmentTemplateId = r.AssignmentTemplateId,
                    AssignmentTitle = r.AssignmentTemplate.Title,
                    Title = r.Title,
                    Description = r.Description
                })
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(RubricViewModel model)
        {
            var total = model.Criteria.Sum(x => x.MaxScore);

            if (total != model.TotalScore)
            {
                return false;
            }
            var rubric = new Rubric
            {
                AssignmentTemplateId = model.AssignmentTemplateId,
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now
            };

            _context.Rubrics.Add(rubric);

            await _context.SaveChangesAsync();

            foreach (var item in model.Criteria)
            {
                _context.RubricCriteria.Add(new RubricCriterion
                {
                    RubricId = rubric.RubricId,
                    CriterionName = item.CriterionName,
                    Description = item.Description,
                    MaxScore = item.MaxScore,
                    DisplayOrder = item.DisplayOrder
                });
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(RubricViewModel model)
        {
            var total = model.Criteria.Sum(x => x.MaxScore);

            if (total != model.TotalScore)
            {
                return false;
            }
            var rubric = await _context.Rubrics
                .Include(r => r.RubricCriteria)
                .FirstOrDefaultAsync(r => r.RubricId == model.RubricId);

            if (rubric == null)
                return false;

            rubric.Title = model.Title;
            rubric.Description = model.Description;
            rubric.UpdatedAt = DateTime.Now;

            _context.RubricCriteria.RemoveRange(rubric.RubricCriteria);

            foreach (var item in model.Criteria)
            {
                rubric.RubricCriteria.Add(new RubricCriterion
                {
                    CriterionName = item.CriterionName,
                    Description = item.Description,
                    MaxScore = item.MaxScore,
                    DisplayOrder = item.DisplayOrder
                });
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long rubricId)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.RubricCriteria)
                .FirstOrDefaultAsync(r => r.RubricId == rubricId);

            if (rubric == null)
                return false;

            _context.RubricCriteria.RemoveRange(rubric.RubricCriteria);

            _context.Rubrics.Remove(rubric);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<RubricViewModel?> GetByIdAsync(long rubricId)
        {
            return await _context.Rubrics
                .Where(r => r.RubricId == rubricId)
                .Select(r => new RubricViewModel
                {
                    RubricId = r.RubricId,
                    AssignmentTemplateId = r.AssignmentTemplateId,
                    AssignmentTitle = r.AssignmentTemplate.Title,
                    TotalScore = r.AssignmentTemplate.TotalScore,
                    Title = r.Title,
                    Description = r.Description,

                    Criteria = r.RubricCriteria
                        .OrderBy(c => c.DisplayOrder)
                        .Select(c => new RubricCriterionViewModel
                        {
                            RubricCriterionId = c.RubricCriterionId,
                            CriterionName = c.CriterionName,
                            Description = c.Description,
                            MaxScore = c.MaxScore,
                            DisplayOrder = c.DisplayOrder
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<RubricViewModel> GetCreateModelAsync(long assignmentId)
        {
            var assignment = await _context.AssignmentTemplates
                .FirstOrDefaultAsync(a => a.AssignmentTemplateId == assignmentId);

            if (assignment == null)
                throw new Exception("Assignment not found.");

            return new RubricViewModel
            {
                AssignmentTemplateId = assignment.AssignmentTemplateId,
                AssignmentTitle = assignment.Title,
                TotalScore = assignment.TotalScore
            };
        }
    }
}