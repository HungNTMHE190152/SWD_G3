namespace EduNexus.ViewModels.Authentication;

public class RegisterResult
{
    public bool Succeeded { get; init; }

    public string? ErrorMessage { get; init; }

    public static RegisterResult Success()
    {
        return new RegisterResult
        {
            Succeeded = true
        };
    }

    public static RegisterResult Failure(string message)
    {
        return new RegisterResult
        {
            Succeeded = false,
            ErrorMessage = message
        };
    }
}