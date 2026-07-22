namespace EduNexus.Areas.Admin.ViewModels.Profile
{
    public class AdminProfileViewModel
    {
        public long UserId { get; set; }

        public string FullName { get; set; } =
            string.Empty;

        public string Email { get; set; } =
            string.Empty;

        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

        public string UserStatus { get; set; } =
            string.Empty;

        public string Username { get; set; } =
            string.Empty;

        public string RoleName { get; set; } =
            string.Empty;

        public bool IsAccountActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }
    }
}