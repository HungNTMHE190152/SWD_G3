// Services/RubricService.cs
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.ViewModels;
using EduNexus.ViewModels.Rubric;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services
{
    public class RubricService : IRubricService
    {
        private readonly EduNexusContext _context;

        public RubricService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<RubricListViewModel>> GetRubricsAsync(long assignmentId)
        {
            return await _context.Rubrics
                .Where(r => r.AssignmentId == assignmentId)
                .Select(r => new RubricListViewModel
                {
                    RubricId = r.RubricId,
                    RubricName = r.RubricName,
                    Description = r.Description,
                    TotalWeight = r.TotalWeight ?? 0
                })
                .ToListAsync();
        }

        public async Task<RubricDetailViewModel?> GetByIdAsync(long rubricId)
        {
            var rubric = await _context.Rubrics
                .FirstOrDefaultAsync(r => r.RubricId == rubricId);

            if (rubric == null) return null;

            return new RubricDetailViewModel
            {
                RubricId = rubric.RubricId,
                AssignmentId = rubric.AssignmentId,
                RubricName = rubric.RubricName,
                Description = rubric.Description,
                TotalWeight = rubric.TotalWeight ?? 0
            };
        }

        public async Task<CreateRubricViewModel> GetCreateModelAsync(long assignmentId)
        {
            var exists = await _context.Rubrics.AnyAsync(r => r.AssignmentId == assignmentId);

            return new CreateRubricViewModel
            {
                AssignmentId = assignmentId
            };
        }

        public async Task CreateAsync(CreateRubricViewModel model)
        {
            var exists = await _context.Rubrics.AnyAsync(r => r.AssignmentId == model.AssignmentId);
            if (exists)
            {
                throw new InvalidOperationException("Assignment này đã tồn tại Rubric. Không thể tạo thêm Rubric mới.");
            }

            var rubric = new Rubric
            {
                AssignmentId = model.AssignmentId,
                RubricName = model.RubricName,
                Description = model.Description,
                TotalWeight = model.TotalWeight
            };

            _context.Rubrics.Add(rubric);
            await _context.SaveChangesAsync();
        }

        public async Task<EditRubricViewModel?> GetEditModelAsync(long rubricId)
        {
            var rubric = await _context.Rubrics.FindAsync(rubricId);
            if (rubric == null) return null;

            return new EditRubricViewModel
            {
                RubricId = rubric.RubricId,
                AssignmentId = rubric.AssignmentId,
                RubricName = rubric.RubricName,
                Description = rubric.Description,
                TotalWeight = rubric.TotalWeight ?? 0
            };
        }

        public async Task UpdateAsync(EditRubricViewModel model)
        {
            var rubric = await _context.Rubrics.FindAsync(model.RubricId);
            if (rubric == null)
            {
                throw new InvalidOperationException("Không tìm thấy Rubric.");
            }

            rubric.RubricName = model.RubricName;
            rubric.Description = model.Description;
            rubric.TotalWeight = model.TotalWeight;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long rubricId)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.RubricCriteria)
                .FirstOrDefaultAsync(r => r.RubricId == rubricId);

            if (rubric != null)
            {
                _context.Rubrics.Remove(rubric);
                await _context.SaveChangesAsync();
            }
        }
    }
}