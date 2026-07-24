using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Assignment.Interfaces;

public interface IEnrollmentService
{
    Task<List<EnrollmentViewModel>> GetByClassroomAsync(
        long classroomId,
        long teacherId);
}