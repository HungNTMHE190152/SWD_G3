using EduNexus.Models;
using Microsoft.AspNetCore.Http;

namespace EduNexus.Services.Interfaces;

public interface IResourceService
{
    Task<List<Resource>> GetByLessonIdAsync(long lessonId);
    Task UploadAsync(long lessonId, IFormFile file);
}