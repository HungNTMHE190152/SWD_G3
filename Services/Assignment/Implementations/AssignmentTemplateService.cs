using EduNexus.Data;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;
using EduNexus.Models;

namespace EduNexus.Services.Assignment.Implementations
{
    public class AssignmentTemplateService : IAssignmentTemplateService
    {
        private readonly EduNexusContext _context;

        public AssignmentTemplateService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<AssignmentTemplateListViewModel>> GetAllAsync(long smeId)
        {
            return await _context.AssignmentTemplates
                .Where(x => x.CreatedBy == smeId)
                .Select(x => new AssignmentTemplateListViewModel
                {
                    AssignmentTemplateId = x.AssignmentTemplateId,
                    Title = x.Title,
                    CourseName = x.Course.Title,
                    ModuleName = x.Module != null ? x.Module.Title : "",
                    TotalScore = x.TotalScore,
                    HasRubric = x.Rubric != null,
                    IsActive = x.IsActive
                })
                .OrderBy(x => x.CourseName)
                .ThenBy(x => x.Title)
                .ToListAsync();
        }

        public async Task<AssignmentTemplateFormViewModel?> GetByIdAsync(long id, long smeId)
        {
            var entity = await _context.AssignmentTemplates
                .FirstOrDefaultAsync(x =>
                    x.AssignmentTemplateId == id &&
                    x.CreatedBy == smeId);

            if (entity == null)
                return null;

            return new AssignmentTemplateFormViewModel
            {
                AssignmentTemplateId = entity.AssignmentTemplateId,
                CourseId = entity.CourseId,
                ModuleId = entity.ModuleId,
                Title = entity.Title,
                Description = entity.Description,
                Instructions = entity.Instructions,
                TotalScore = entity.TotalScore,
                IsActive = entity.IsActive
            };
        }

        public async Task<bool> CreateAsync(
            AssignmentTemplateFormViewModel model,
            long smeId)
        {
            var entity = new AssignmentTemplate
            {
                CourseId = model.CourseId,
                ModuleId = model.ModuleId,
                CreatedBy = smeId,
                Title = model.Title,
                Description = model.Description,
                Instructions = model.Instructions,
                TotalScore = model.TotalScore,
                IsActive = model.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.AssignmentTemplates.Add(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(
            AssignmentTemplateFormViewModel model,
            long smeId)
        {
            var entity = await _context.AssignmentTemplates
                .FirstOrDefaultAsync(x =>
                    x.AssignmentTemplateId == model.AssignmentTemplateId &&
                    x.CreatedBy == smeId);

            if (entity == null)
                return false;

            entity.CourseId = model.CourseId;
            entity.ModuleId = model.ModuleId;
            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.Instructions = model.Instructions;
            entity.TotalScore = model.TotalScore;
            entity.IsActive = model.IsActive;
            entity.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long id, long smeId)
        {
            var entity = await _context.AssignmentTemplates
                .FirstOrDefaultAsync(x =>
                    x.AssignmentTemplateId == id &&
                    x.CreatedBy == smeId);

            if (entity == null)
                return false;

            entity.IsActive = false;
            entity.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<CourseOptionViewModel>> GetCoursesAsync(long smeId)
        {
            return await _context.Courses
                .Where(x => x.CreatedBy == smeId)
                .OrderBy(x => x.Title)
                .Select(x => new CourseOptionViewModel
                {
                    CourseId = x.CourseId,
                    CourseCode = x.CourseCode,
                    CourseTitle = x.Title
                })
                .ToListAsync();
        }

        public async Task<List<ModuleOptionViewModel>> GetModulesAsync(long courseId)
        {
            return await _context.Modules
                .Where(x => x.CourseId == courseId)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new ModuleOptionViewModel
                {
                    ModuleId = x.ModuleId,
                    ModuleTitle = x.Title
                })
                .ToListAsync();
        }
    }
}