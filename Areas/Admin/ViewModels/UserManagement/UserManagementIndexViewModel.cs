namespace EduNexus.Areas.Admin.ViewModels.UserManagement
{
    public class UserManagementIndexViewModel
    {
        public UserFilterViewModel Filter { get; set; } = new();

        public List<UserListItemViewModel> Users { get; set; } = new();

        public List<RoleOptionViewModel> Roles { get; set; } = new();

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage =>
            Filter.Page > 1;

        public bool HasNextPage =>
            Filter.Page < TotalPages;
    }
}