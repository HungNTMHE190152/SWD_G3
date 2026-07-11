using EduNexus.Models;
using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<List<AssignmentListViewModel>> GetAllAssignmentsAsync();
        Task<AssignmentDetailViewModel?> GetAssignmentByIdAsync(long id);

        Task CreateAssignmentAsync(CreateAssignmentViewModel model);
        Task<EditAssignmentViewModel?> GetAssignmentForEditAsync(long id);

        Task UpdateAssignmentAsync(EditAssignmentViewModel model);

        Task DeleteAssignmentAsync(long id);
    }
}
