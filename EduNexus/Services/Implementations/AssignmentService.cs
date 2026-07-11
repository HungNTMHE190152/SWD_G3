using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly EduNexusContext _context;

        public AssignmentService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<AssignmentListViewModel>> GetAllAssignmentsAsync()
        {
            return await _context.Assignments
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .Include(a => a.CreatedByNavigation)
                .Select(a => new AssignmentListViewModel
                {
                    AssignmentId = a.AssignmentId,
                    Title = a.Title,
                    CourseName = a.Module.Course.Title,
                    ModuleName = a.Module.ModuleName,
                    AssignmentType = a.AssignmentType,
                    TotalScore = a.TotalScore,
                    OpenDate = a.OpenDate,
                    DueDate = a.DueDate,
                    Status = a.Status,
                    CreatedBy = a.CreatedByNavigation.FullName
                })
                .ToListAsync();
        }
        public async Task<AssignmentDetailViewModel?> GetAssignmentByIdAsync(long id)
        {
            return await _context.Assignments
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .Include(a => a.CreatedByNavigation)
                .Where(a => a.AssignmentId == id)
                .Select(a => new AssignmentDetailViewModel
                {
                    AssignmentId = a.AssignmentId,
                    Title = a.Title,
                    Description = a.Description,
                    CourseName = a.Module.Course.Title,
                    ModuleName = a.Module.ModuleName,
                    AssignmentType = a.AssignmentType,
                    TotalScore = a.TotalScore,
                    OpenDate = a.OpenDate,
                    DueDate = a.DueDate,
                    AllowLateSubmission = a.AllowLateSubmission,
                    IsAIGenerated = a.IsAigenerated,
                    Status = a.Status,
                    CreatedBy = a.CreatedByNavigation.FullName
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAssignmentAsync(CreateAssignmentViewModel model)
        {
            Assignment assignment = new Assignment
            {
                Title = model.Title,
                Description = model.Description,
                ModuleId = model.ModuleId,
                AssignmentType = model.AssignmentType,
                TotalScore = model.TotalScore,
                OpenDate = model.OpenDate,
                DueDate = model.DueDate,
                AllowLateSubmission = model.AllowLateSubmission,
                CreatedAt = DateTime.Now,
                Status = "Draft",

                // Tạm thời
                CreatedBy = 1
            };

            _context.Assignments.Add(assignment);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssignmentAsync(long id)
        {
            var assignment = await _context.Assignments.FindAsync(id);

            if (assignment == null)
                return;

            _context.Assignments.Remove(assignment);

            await _context.SaveChangesAsync();
        }

        public async Task<EditAssignmentViewModel?> GetAssignmentForEditAsync(long id)
        {
            return await _context.Assignments
                .Where(a => a.AssignmentId == id)
                .Select(a => new EditAssignmentViewModel
                {
                    AssignmentId = a.AssignmentId,
                    Title = a.Title,
                    Description = a.Description,
                    ModuleId = a.ModuleId,
                    AssignmentType = a.AssignmentType,
                    TotalScore = a.TotalScore,
                    OpenDate = a.OpenDate,
                    DueDate = a.DueDate,
                    AllowLateSubmission = a.AllowLateSubmission ?? false,
                    IsAIGenerated = a.IsAigenerated ?? false,
                    Status = a.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAssignmentAsync(EditAssignmentViewModel model)
        {
            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.AssignmentId == model.AssignmentId);

            if (assignment == null)
            {
                throw new Exception("Assignment not found.");
            }

            assignment.Title = model.Title;
            assignment.Description = model.Description;
            assignment.ModuleId = model.ModuleId;
            assignment.AssignmentType = model.AssignmentType;
            assignment.TotalScore = model.TotalScore;
            assignment.OpenDate = model.OpenDate;
            assignment.DueDate = model.DueDate;
            assignment.AllowLateSubmission = model.AllowLateSubmission;
            assignment.IsAigenerated = model.IsAIGenerated;
            assignment.Status = model.Status;

            await _context.SaveChangesAsync();
        }
    }
}