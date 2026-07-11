using EduNexus.ViewModels.Assignment;
using EduNexus.ViewModels.Grading;
using EduNexus.ViewModels.Submission;

namespace EduNexus.Services.Interfaces
{
    public interface IGradingService
    {
        Task<List<AssignmentGradingListViewModel>> GetAssignmentsWithSubmissionsAsync();
        Task<List<SubmissionListViewModel>> GetSubmissionsAsync(long assignmentId);
        Task<GradeSubmissionViewModel?> GetGradingAsync(long submissionId);
        Task SaveGradeAsync(GradeSubmissionViewModel model);
        Task PublishGradeAsync(long submissionId);
        Task UnpublishGradeAsync(long submissionId);
        Task<GradeSubmissionViewModel?> GetSubmissionForGradingAsync(long submissionId);
        Task SaveGradeAsync(GradeSubmissionViewModel model, long graderId);
    }
}
