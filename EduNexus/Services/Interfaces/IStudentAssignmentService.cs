using EduNexus.Models;
using EduNexus.ViewModels.Student;

namespace EduNexus.Services.Interfaces
{
    public interface IStudentAssignmentService
    {
        Task<List<StudentAssignmentListViewModel>> GetAssignmentsAsync(long studentId);
        Task<StudentAssignmentDetailViewModel?> GetAssignmentDetailAsync(long assignmentId, long studentId);
        Task<StudentAssignmentWorkspaceViewModel?> StartAssignmentAsync(long assignmentId, long studentId);
        Task SaveDraftAsync(StudentAssignmentWorkspaceViewModel model, long studentId);
        Task<bool> SubmitAssignmentAsync(long assignmentId, long studentId);
        Task<List<StudentSubmissionHistoryViewModel>> GetSubmissionHistoryAsync(long studentId);
        Task<StudentSubmissionDetailViewModel?> GetSubmissionDetailAsync(long submissionId, long studentId);
        Task<EssaySubmission?> GetLatestSubmissionAsync(long assignmentId, long studentId);


    }
}
