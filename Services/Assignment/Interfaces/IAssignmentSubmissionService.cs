using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Assignment.Interfaces
{
    public interface IAssignmentSubmissionService
    {
        Task<List<AssignmentSubmissionViewModel>> GetByAssignmentAsync(
            long classroomAssignmentId,
            long teacherId);
    }
}