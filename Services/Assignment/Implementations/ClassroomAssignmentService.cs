using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Assignment.Implementations;

public class ClassroomAssignmentService : IClassroomAssignmentService
{
    private readonly EduNexusContext _context;

    public ClassroomAssignmentService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<ClassroomAssignmentViewModel>> GetAllAsync(long teacherId)
    {
        return await _context.ClassroomAssignments
            .Include(x => x.Classroom)
            .Include(x => x.AssignmentTemplate)
            .Where(x => x.PublishedBy == teacherId)
            .Select(x => new ClassroomAssignmentViewModel
            {
                ClassroomAssignmentId = x.ClassroomAssignmentId,

                ClassroomId = x.ClassroomId,
                AssignmentTemplateId = x.AssignmentTemplateId,

                ClassroomName = x.Classroom.ClassName,
                AssignmentTemplateName = x.AssignmentTemplate.Title,

                Title = x.Title,

                OpenDate = x.OpenDate,
                DueDate = x.DueDate,

                AllowLateSubmission = x.AllowLateSubmission,

                Status = x.Status
            })
            .ToListAsync();
    }

    public async Task<ClassroomAssignmentViewModel> GetCreateModelAsync(long teacherId)
        {
        var model = new ClassroomAssignmentViewModel();

        model.Classrooms = await _context.Classrooms
            .Where(c => c.TeacherId == teacherId)
            .Select(c => new SelectListItem
            {
                Value = c.ClassroomId.ToString(),
                Text = c.ClassName
            })
            .ToListAsync();

        model.Assignments = await _context.AssignmentTemplates
            .Where(a => a.IsActive)
            .Select(a => new SelectListItem
            {
                Value = a.AssignmentTemplateId.ToString(),
                Text = a.Title
            })
            .ToListAsync();

        return model;
    }

    // Các method còn lại sẽ làm ở bước tiếp theo
    public async Task<bool> CreateAsync(ClassroomAssignmentViewModel model, long publishedBy)
    {
        if (model.OpenDate >= model.DueDate)
            return false;

        var assignment = new ClassroomAssignment
        {
            ClassroomId = model.ClassroomId,
            AssignmentTemplateId = model.AssignmentTemplateId,
            PublishedBy = publishedBy,
            Title = model.Title,
            OpenDate = model.OpenDate,
            DueDate = model.DueDate,
            AllowLateSubmission = model.AllowLateSubmission,
            Status = model.Status,
            PublishedAt = DateTime.Now,
            CreatedAt = DateTime.Now
        };

        _context.ClassroomAssignments.Add(assignment);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ClassroomAssignmentViewModel?> GetByIdAsync(long id)
    {
        var assignment = await _context.ClassroomAssignments
            .Include(x => x.Classroom)
            .Include(x => x.AssignmentTemplate)
            .FirstOrDefaultAsync(x => x.ClassroomAssignmentId == id);

        if (assignment == null)
            return null;

        var model = new ClassroomAssignmentViewModel
        {
            ClassroomAssignmentId = assignment.ClassroomAssignmentId,

            ClassroomId = assignment.ClassroomId,
            AssignmentTemplateId = assignment.AssignmentTemplateId,

            ClassroomName = assignment.Classroom.ClassName,
            AssignmentTemplateName = assignment.AssignmentTemplate.Title,

            Title = assignment.Title,
            OpenDate = assignment.OpenDate,
            DueDate = assignment.DueDate,
            AllowLateSubmission = assignment.AllowLateSubmission,
            Status = assignment.Status
        };

        model.Classrooms = await _context.Classrooms
            .Select(c => new SelectListItem
            {
                Value = c.ClassroomId.ToString(),
                Text = c.ClassName
            })
            .ToListAsync();

        model.Assignments = await _context.AssignmentTemplates
            .Where(a => a.IsActive)
            .Select(a => new SelectListItem
            {
                Value = a.AssignmentTemplateId.ToString(),
                Text = a.Title
            })
            .ToListAsync();

        return model;
    }
    public async Task<bool> UpdateAsync(ClassroomAssignmentViewModel model)
    {
        if (model.OpenDate >= model.DueDate)
            return false;

        var assignment = await _context.ClassroomAssignments
            .FirstOrDefaultAsync(x => x.ClassroomAssignmentId == model.ClassroomAssignmentId);

        if (assignment == null)
            return false;

        assignment.ClassroomId = model.ClassroomId;
        assignment.AssignmentTemplateId = model.AssignmentTemplateId;
        assignment.Title = model.Title;
        assignment.OpenDate = model.OpenDate;
        assignment.DueDate = model.DueDate;
        assignment.AllowLateSubmission = model.AllowLateSubmission;
        assignment.Status = model.Status;
        assignment.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> DeleteAsync(long id)
    {
        var assignment = await _context.ClassroomAssignments
            .FirstOrDefaultAsync(x => x.ClassroomAssignmentId == id);

        if (assignment == null)
            return false;

        _context.ClassroomAssignments.Remove(assignment);

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task PopulateDropdownsAsync(ClassroomAssignmentViewModel model)
    {
        model.Classrooms = await _context.Classrooms
            .Select(c => new SelectListItem
            {
                Value = c.ClassroomId.ToString(),
                Text = c.ClassName
            })
            .ToListAsync();

        model.Assignments = await _context.AssignmentTemplates
            .Where(a => a.IsActive)
            .Select(a => new SelectListItem
            {
                Value = a.AssignmentTemplateId.ToString(),
                Text = a.Title
            })
            .ToListAsync();
    }
}