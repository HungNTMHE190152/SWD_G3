using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Assignment.Interfaces
{
    public interface ISubmissionGradeService
    {
        Task<SubmissionGradeViewModel?> GetSubmissionAsync(long submissionId);

        Task SaveGradeAsync(SubmissionGradeViewModel model, long teacherId);
    }
}