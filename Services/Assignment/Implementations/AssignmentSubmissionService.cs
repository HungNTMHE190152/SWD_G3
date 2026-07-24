using EduNexus.Data;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Assignment.Implementations;

public class AssignmentSubmissionService : IAssignmentSubmissionService
{
    private readonly EduNexusContext _context;

    public AssignmentSubmissionService(EduNexusContext context)
    {
        _context = context;
    }
    public async Task<List<AssignmentSubmissionViewModel>> GetByAssignmentAsync(
     long classroomAssignmentId,
     long teacherId)
    {
        return await _context.AssignmentSubmissions
            .Include(x => x.Student)
            .Include(x => x.ClassroomAssignment)
            .Include(x => x.SubmissionGrade)
            .Where(x =>
                x.ClassroomAssignmentId == classroomAssignmentId &&
                x.ClassroomAssignment.PublishedBy == teacherId)
            .Select(x => new AssignmentSubmissionViewModel
            {
                SubmissionId = x.AssignmentSubmissionId,

                ClassroomAssignmentId = x.ClassroomAssignmentId,

                AssignmentTitle = x.ClassroomAssignment.Title,

                StudentName = x.Student.FullName,

                SubmittedAt = x.SubmittedAt,

                Status = x.Status,

                Score = x.SubmissionGrade != null
                    ? (double?)x.SubmissionGrade.Score
                    : null,

                IsLate =
                    x.SubmittedAt != null &&
                    x.SubmittedAt > x.ClassroomAssignment.DueDate
            })
            .OrderByDescending(x => x.SubmittedAt)
            .ToListAsync();
    }
}