using EduNexus.Areas.Admin.ViewModels.UserManagement;
using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Administration.Implementations
{
    public class UserManagementService : IUserManagementService
    {
        private readonly EduNexusContext _context;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public UserManagementService(
            EduNexusContext context,
            IPasswordHasher<Account> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserManagementIndexViewModel> GetUsersAsync(
            UserFilterViewModel filter)
        {
            filter.Page = filter.Page < 1
                ? 1
                : filter.Page;

            filter.PageSize =
                filter.PageSize is < 5 or > 100
                    ? 10
                    : filter.PageSize;

            var query =
                from user in _context.Users.AsNoTracking()
                join account in _context.Accounts.AsNoTracking()
                    on user.UserId equals account.UserId
                join role in _context.Roles.AsNoTracking()
                    on account.RoleId equals role.RoleId
                select new
                {
                    User = user,
                    Account = account,
                    Role = role
                };

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                string keyword = filter.SearchText.Trim();

                query = query.Where(item =>
                    item.User.FullName.Contains(keyword)
                    || item.User.Email.Contains(keyword)
                    || item.Account.Username.Contains(keyword));
            }

            if (filter.RoleId.HasValue)
            {
                query = query.Where(item =>
                    item.Account.RoleId == filter.RoleId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.UserStatus))
            {
                query = query.Where(item =>
                    item.User.Status == filter.UserStatus);
            }

            if (filter.IsAccountActive.HasValue)
            {
                query = query.Where(item =>
                    item.Account.IsActive ==
                    filter.IsAccountActive.Value);
            }

            int totalItems = await query.CountAsync();

            int totalPages = totalItems == 0
                ? 1
                : (int)Math.Ceiling(
                    totalItems / (double)filter.PageSize);

            if (filter.Page > totalPages)
            {
                filter.Page = totalPages;
            }

            List<UserListItemViewModel> users =
                await query
                    .OrderByDescending(item =>
                        item.User.CreatedAt)
                    .Skip(
                        (filter.Page - 1)
                        * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(item =>
                        new UserListItemViewModel
                        {
                            UserId =
                                item.User.UserId,

                            AccountId =
                                item.Account.AccountId,

                            FullName =
                                item.User.FullName,

                            Email =
                                item.User.Email,

                            Username =
                                item.Account.Username,

                            RoleName =
                                item.Role.RoleName,

                            UserStatus =
                                item.User.Status,

                            IsAccountActive =
                                item.Account.IsActive,

                            CreatedAt =
                                item.User.CreatedAt,

                            LastLoginAt =
                                item.Account.LastLoginAt
                        })
                    .ToListAsync();

            return new UserManagementIndexViewModel
            {
                Filter = filter,
                Users = users,
                Roles = await GetRolesAsync(),
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<UserDetailsViewModel?> GetDetailsAsync(
            long userId)
        {
            UserDetailsViewModel? userDetails =
                await (
                    from user in _context.Users.AsNoTracking()
                    join account in
                        _context.Accounts.AsNoTracking()
                        on user.UserId equals account.UserId
                    join role in _context.Roles.AsNoTracking()
                        on account.RoleId equals role.RoleId
                    where user.UserId == userId
                    select new UserDetailsViewModel
                    {
                        UserId = user.UserId,
                        AccountId = account.AccountId,

                        FullName = user.FullName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        AvatarUrl = user.AvatarUrl,
                        UserStatus = user.Status,
                        UserCreatedAt = user.CreatedAt,
                        UserUpdatedAt = user.UpdatedAt,

                        Username = account.Username,
                        RoleId = account.RoleId,
                        RoleName = role.RoleName,
                        IsAccountActive = account.IsActive,
                        AccountCreatedAt = account.CreatedAt,
                        AccountUpdatedAt = account.UpdatedAt,
                        LastLoginAt = account.LastLoginAt
                    }
                ).FirstOrDefaultAsync();

            if (userDetails != null)
            {
                userDetails.Roles =
                    await GetRolesAsync();
            }

            return userDetails;
        }

        public async Task<List<RoleOptionViewModel>> GetRolesAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .OrderBy(role => role.RoleName)
                .Select(role =>
                    new RoleOptionViewModel
                    {
                        RoleId = role.RoleId,
                        RoleName = role.RoleName
                    })
                .ToListAsync();
        }

        public async Task<ServiceResult<long>> CreateUserAsync(
            CreateUserViewModel model)
        {
            string fullName = model.FullName.Trim();
            string email = model.Email.Trim();
            string username = model.Username.Trim();

            bool emailExists = await _context.Users
                .AnyAsync(user =>
                    user.Email == email);

            if (emailExists)
            {
                return ServiceResult<long>.Failure(
                    "This email address is already in use.");
            }

            bool usernameExists = await _context.Accounts
                .AnyAsync(account =>
                    account.Username == username);

            if (usernameExists)
            {
                return ServiceResult<long>.Failure(
                    "This username is already in use.");
            }

            Role? role = await _context.Roles
                .FirstOrDefaultAsync(role =>
                    role.RoleId == model.RoleId);

            if (role == null)
            {
                return ServiceResult<long>.Failure(
                    "The selected role does not exist.");
            }

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                User user = new User
                {
                    FullName = fullName,
                    Email = email,
                    PhoneNumber =
                        string.IsNullOrWhiteSpace(
                            model.PhoneNumber)
                            ? null
                            : model.PhoneNumber.Trim(),

                    AvatarUrl =
                        string.IsNullOrWhiteSpace(
                            model.AvatarUrl)
                            ? null
                            : model.AvatarUrl.Trim(),

                    Status = UserStatuses.Active,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                Account account = new Account
                {
                    UserId = user.UserId,
                    RoleId = model.RoleId,
                    Username = username,
                    PasswordHash = string.Empty,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                account.PasswordHash =
                    _passwordHasher.HashPassword(
                        account,
                        model.Password);

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return ServiceResult<long>.Success(
                    user.UserId,
                    "The user account was created successfully.");
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();

                return ServiceResult<long>.Failure(
                    "The account could not be created because "
                    + "the email or username may already exist.");
            }
            catch
            {
                await transaction.RollbackAsync();

                return ServiceResult<long>.Failure(
                    "An unexpected error occurred while "
                    + "creating the account.");
            }
        }

        public async Task<ServiceResult> SetAccountActiveAsync(
            long userId,
            bool isActive,
            long currentAdminUserId)
        {
            Account? account = await _context.Accounts
                .Include(account => account.Role)
                .FirstOrDefaultAsync(account =>
                    account.UserId == userId);

            if (account == null)
            {
                return ServiceResult.Failure(
                    "The account was not found.");
            }

            if (userId == currentAdminUserId)
            {
                return ServiceResult.Failure(
                    "You cannot lock or unlock "
                    + "your own account.");
            }

            if (!isActive
                && account.Role.RoleName == RoleNames.Admin
                && await IsLastActiveAdminAsync(
                    account.AccountId))
            {
                return ServiceResult.Failure(
                    "The last active Admin account "
                    + "cannot be locked.");
            }

            account.IsActive = isActive;
            account.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ServiceResult.Success(
                isActive
                    ? "The account has been unlocked."
                    : "The account has been locked.");
        }

        public async Task<ServiceResult> ChangeRoleAsync(
            long userId,
            long newRoleId,
            long currentAdminUserId)
        {
            Account? account = await _context.Accounts
                .Include(account => account.Role)
                .FirstOrDefaultAsync(account =>
                    account.UserId == userId);

            if (account == null)
            {
                return ServiceResult.Failure(
                    "The account was not found.");
            }

            if (userId == currentAdminUserId)
            {
                return ServiceResult.Failure(
                    "You cannot change your own role.");
            }

            Role? newRole = await _context.Roles
                .FirstOrDefaultAsync(role =>
                    role.RoleId == newRoleId);

            if (newRole == null)
            {
                return ServiceResult.Failure(
                    "The selected role does not exist.");
            }

            if (account.RoleId == newRoleId)
            {
                return ServiceResult.Failure(
                    "The account already has this role.");
            }

            if (account.Role.RoleName == RoleNames.Admin
                && newRole.RoleName != RoleNames.Admin
                && await IsLastActiveAdminAsync(
                    account.AccountId))
            {
                return ServiceResult.Failure(
                    "The last active Admin account "
                    + "cannot be assigned another role.");
            }

            account.RoleId = newRoleId;
            account.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ServiceResult.Success(
                $"The role was changed to {newRole.RoleName}.");
        }

        private async Task<bool> IsLastActiveAdminAsync(
            long accountId)
        {
            int activeAdminCount =
                await (
                    from account in _context.Accounts
                    join role in _context.Roles
                        on account.RoleId equals role.RoleId
                    where role.RoleName == RoleNames.Admin
                          && account.IsActive
                    select account.AccountId
                ).CountAsync();

            Account? selectedAccount =
                await _context.Accounts
                    .Include(account => account.Role)
                    .FirstOrDefaultAsync(account =>
                        account.AccountId == accountId);

            return selectedAccount != null
                   && selectedAccount.IsActive
                   && selectedAccount.Role.RoleName ==
                       RoleNames.Admin
                   && activeAdminCount <= 1;
        }
    }
}