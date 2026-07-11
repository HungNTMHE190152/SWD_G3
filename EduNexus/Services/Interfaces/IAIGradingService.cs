using EduNexus.ViewModels.Student;

namespace EduNexus.Services.Interfaces
{
    public interface IAIGradingService
    {
        Task<AISubmissionResultViewModel> GradeSubmissionAsync(long submissionId);
       
    }
}