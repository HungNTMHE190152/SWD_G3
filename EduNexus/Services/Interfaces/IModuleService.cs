using EduNexus.Models;

namespace EduNexus.Services.Interfaces;

public interface IModuleService
{
    Task<Module?> GetByIdAsync(long moduleId);
    Task CreateModuleAsync(Module module);
}