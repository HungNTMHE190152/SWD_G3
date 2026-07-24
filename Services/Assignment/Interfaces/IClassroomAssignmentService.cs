using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Assignment.Interfaces;

public interface IClassroomAssignmentService
{
    Task<List<ClassroomAssignmentViewModel>> GetAllAsync(long teacherId);

    Task<ClassroomAssignmentViewModel> GetCreateModelAsync(long teacherId);

    Task<bool> CreateAsync(ClassroomAssignmentViewModel model, long publishedBy);

    Task<ClassroomAssignmentViewModel?> GetByIdAsync(long id);

    Task<bool> UpdateAsync(ClassroomAssignmentViewModel model);

    Task<bool> DeleteAsync(long id);
    Task PopulateDropdownsAsync(ClassroomAssignmentViewModel model);
}