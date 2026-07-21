namespace EduNexus.Areas.Admin.ViewModels.UserManagement
{
    public class UserDetailsViewModel
    {
        public long UserId { get; set; }

        public long AccountId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

        public string UserStatus { get; set; } = string.Empty;

        public DateTime UserCreatedAt { get; set; }

        public DateTime? UserUpdatedAt { get; set; }

        public string Username { get; set; } = string.Empty;

        public long RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public bool IsAccountActive { get; set; }

        public DateTime AccountCreatedAt { get; set; }

        public DateTime? AccountUpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public List<RoleOptionViewModel> Roles { get; set; } = new();
    }
}