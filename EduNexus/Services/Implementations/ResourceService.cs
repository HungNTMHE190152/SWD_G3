using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class ResourceService : IResourceService
{
    private readonly EduNexusContext _context;
    private readonly IWebHostEnvironment _env;

    public ResourceService(EduNexusContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<List<Resource>> GetByLessonIdAsync(long lessonId)
    {
        return await _context.Resources
            .Where(r => r.LessonId == lessonId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task UploadAsync(long lessonId, IFormFile file)
    {
        if (file == null || file.Length == 0) return;

        var folder = Path.Combine(_env.WebRootPath, "uploads", "resources");
        Directory.CreateDirectory(folder);

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(folder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var resource = new Resource
        {
            LessonId = lessonId,
            ResourceName = file.FileName,
            FileUrl = "/uploads/resources/" + fileName,
            FileType = file.ContentType,
            FileSize = file.Length,
            CreatedAt = DateTime.Now
        };

        _context.Resources.Add(resource);
        await _context.SaveChangesAsync();
    }
}