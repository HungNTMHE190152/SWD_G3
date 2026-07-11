using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;

namespace EduNexus.Services.Implementations;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.GetCurrentUserId() ?? 0;
    }

    public string GetCurrentRole()
    {
        return _httpContextAccessor.HttpContext?.User.GetCurrentRole() ?? string.Empty;
    }

    public string GetCurrentEmail()
    {
        return _httpContextAccessor.HttpContext?.User.GetCurrentEmail() ?? string.Empty;
    }

    public string GetCurrentFullName()
    {
        return _httpContextAccessor.HttpContext?.User.GetCurrentFullName() ?? string.Empty;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public bool IsStudent()
    {
        return GetCurrentRole() == RoleNames.Student;
    }

    public bool IsSme()
    {
        return GetCurrentRole() == RoleNames.Sme;
    }
}