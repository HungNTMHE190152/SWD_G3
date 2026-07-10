using EduNexus.Models;

namespace EduNexus.Services.Interfaces;

public interface ICourseService
{
    Task<List<Course>> GetCoursesBySmeAsync(long smeId);
    Task<Course?> GetCourseStructureAsync(long courseId);
}