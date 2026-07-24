using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Assignment.Interfaces;

public interface IClassroomService
{
    Task<List<ClassroomViewModel>> GetAllAsync(long teacherId);

    Task<ClassroomViewModel> GetCreateModelAsync(long teacherId);

    Task<ClassroomViewModel?> GetByIdAsync(long id, long teacherId);

    Task<bool> CreateAsync(ClassroomViewModel model, long teacherId);

    Task<bool> UpdateAsync(ClassroomViewModel model, long teacherId);

    Task<bool> DeleteAsync(long id, long teacherId);
}