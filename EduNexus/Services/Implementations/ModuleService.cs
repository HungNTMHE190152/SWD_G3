using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;

namespace EduNexus.Services.Implementations;

public class ModuleService : IModuleService
{
    private readonly EduNexusContext _context;

    public ModuleService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<Module?> GetByIdAsync(long moduleId)
    {
        return await _context.Modules.FindAsync(moduleId);
    }

    public async Task CreateModuleAsync(Module module)
    {
        _context.Modules.Add(module);
        await _context.SaveChangesAsync();
    }
}