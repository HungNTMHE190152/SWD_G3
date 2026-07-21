namespace EduNexus.Areas.Admin.ViewModels.UserManagement
{
    public class UserFilterViewModel
    {
        public string? SearchText { get; set; }

        public long? RoleId { get; set; }

        public string? UserStatus { get; set; }

        public bool? IsAccountActive { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}