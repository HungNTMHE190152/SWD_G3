using EduNexus.Data;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Assignment.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly EduNexusContext _context;

    public EnrollmentService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<EnrollmentViewModel>> GetByClassroomAsync(
        long classroomId,
        long teacherId)
    {
        return await _context.Enrollments
            .Include(x => x.Student)
            .Include(x => x.Classroom)
            .Where(x =>
                x.ClassroomId == classroomId &&
                x.Classroom.TeacherId == teacherId)
            .OrderBy(x => x.Student.FullName)
            .Select(x => new EnrollmentViewModel
            {
                EnrollmentId = x.EnrollmentId,

                ClassroomId = x.ClassroomId,
                ClassroomName = x.Classroom.ClassName,

                StudentId = x.StudentId,
                StudentName = x.Student.FullName,
                StudentEmail = x.Student.Email,

                EnrolledAt = x.EnrolledAt,

                Status = x.Status
            })
            .ToListAsync();
    }
}