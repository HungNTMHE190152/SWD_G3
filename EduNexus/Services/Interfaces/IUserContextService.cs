namespace EduNexus.Services.Interfaces;

public interface IUserContextService
{
    long GetCurrentUserId();

    string GetCurrentRole();

    string GetCurrentEmail();

    string GetCurrentFullName();

    bool IsAuthenticated();

    bool IsStudent();

    bool IsSme();
}