namespace EduNexus.ViewModels.Authentication
{
    public class LoginResult
    {
        public bool Succeeded { get; private set; }

        public string? ErrorMessage { get; private set; }

        public long AccountId { get; private set; }

        public long UserId { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string RoleName { get; private set; } = string.Empty;

        public static LoginResult Success(
            long accountId,
            long userId,
            string fullName,
            string email,
            string roleName)
        {
            return new LoginResult
            {
                Succeeded = true,
                AccountId = accountId,
                UserId = userId,
                FullName = fullName,
                Email = email,
                RoleName = roleName
            };
        }

        public static LoginResult Failure(string errorMessage)
        {
            return new LoginResult
            {
                Succeeded = false,
                ErrorMessage = errorMessage
            };
        }
    }
}