using System.Security.Claims;

namespace EduNexus.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static long GetCurrentUserId(
            this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!long.TryParse(value, out var userId))
            {
                throw new UnauthorizedAccessException(
                    "The current user ID is missing or invalid.");
            }

            return userId;
        }

        public static string GetCurrentRole(
            this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role)
                ?? string.Empty;
        }
    }
}