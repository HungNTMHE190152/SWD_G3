using System.Security.Claims;

namespace EduNexus.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static long GetCurrentUserId(this ClaimsPrincipal user)
    {
        string? value = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (long.TryParse(value, out long userId))
        {
            return userId;
        }

        return 0;
    }

    public static string GetCurrentRole(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }

    public static string GetCurrentEmail(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    }

    public static string GetCurrentFullName(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    }
}