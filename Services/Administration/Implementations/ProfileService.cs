using System.Text.RegularExpressions;
using EduNexus.Data;
using EduNexus.Services.Common;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations
{
    public class ProfileService
        : IProfileService
    {
        private readonly EduNexusContext _context;

        public ProfileService(
            EduNexusContext context)
        {
            _context = context;
        }

        public async Task<UserProfileViewModel?>
            GetProfileAsync(long userId)
        {
            if (userId <= 0)
            {
                return null;
            }

            return await (
                from user in
                    _context.Users.AsNoTracking()

                join account in
                    _context.Accounts.AsNoTracking()
                    on user.UserId
                    equals account.UserId

                join role in
                    _context.Roles.AsNoTracking()
                    on account.RoleId
                    equals role.RoleId

                where user.UserId == userId

                select new UserProfileViewModel
                {
                    UserId =
                        user.UserId,

                    FullName =
                        user.FullName,

                    Email =
                        user.Email,

                    PhoneNumber =
                        user.PhoneNumber,

                    AvatarUrl =
                        user.AvatarUrl,

                    UserStatus =
                        user.Status,

                    Username =
                        account.Username,

                    RoleName =
                        role.RoleName,

                    IsAccountActive =
                        account.IsActive,

                    CreatedAt =
                        user.CreatedAt,

                    UpdatedAt =
                        user.UpdatedAt,

                    LastLoginAt =
                        account.LastLoginAt
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<EditProfileViewModel?>
            GetEditAsync(long userId)
        {
            if (userId <= 0)
            {
                return null;
            }

            return await (
                from user in
                    _context.Users.AsNoTracking()

                join account in
                    _context.Accounts.AsNoTracking()
                    on user.UserId
                    equals account.UserId

                where user.UserId == userId

                select new EditProfileViewModel
                {
                    FullName =
                        user.FullName,

                    PhoneNumber =
                        user.PhoneNumber,

                    AvatarUrl =
                        user.AvatarUrl
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<ServiceResult>
            UpdateProfileAsync(
                long userId,
                EditProfileViewModel model)
        {
            if (userId <= 0)
            {
                return ServiceResult.Failure(
                    "The current user could not be identified.");
            }

            string fullName =
                model.FullName?.Trim()
                ?? string.Empty;

            string? phoneNumber =
                string.IsNullOrWhiteSpace(
                    model.PhoneNumber)
                    ? null
                    : model.PhoneNumber.Trim();

            string? avatarUrl =
                string.IsNullOrWhiteSpace(
                    model.AvatarUrl)
                    ? null
                    : model.AvatarUrl.Trim();

            if (fullName.Length < 2
                || fullName.Length > 150)
            {
                return ServiceResult.Failure(
                    "Full name must contain between "
                    + "2 and 150 characters.");
            }

            if (phoneNumber != null)
            {
                if (phoneNumber.Length > 30)
                {
                    return ServiceResult.Failure(
                        "Phone number cannot exceed "
                        + "30 characters.");
                }

                if (!Regex.IsMatch(
                    phoneNumber,
                    @"^[0-9+\-\s()]+$"))
                {
                    return ServiceResult.Failure(
                        "Phone number contains "
                        + "invalid characters.");
                }
            }

            if (avatarUrl != null)
            {
                if (avatarUrl.Length > 500)
                {
                    return ServiceResult.Failure(
                        "Avatar URL cannot exceed "
                        + "500 characters.");
                }

                if (!IsValidAvatarUrl(avatarUrl))
                {
                    return ServiceResult.Failure(
                        "Avatar URL must be an HTTP/HTTPS URL "
                        + "or a local path beginning with '/'.");
                }
            }

            var profileRecord =
                await (
                    from user in _context.Users

                    join account in _context.Accounts
                        on user.UserId
                        equals account.UserId

                    where user.UserId == userId

                    select new
                    {
                        User = user,
                        Account = account
                    }
                ).FirstOrDefaultAsync();

            if (profileRecord == null)
            {
                return ServiceResult.Failure(
                    "The user profile was not found.");
            }

            if (!profileRecord.Account.IsActive)
            {
                return ServiceResult.Failure(
                    "The current account is inactive.");
            }

            if (profileRecord.User.Status != "ACTIVE")
            {
                return ServiceResult.Failure(
                    "The current user profile is inactive.");
            }

            bool hasChanges =
                profileRecord.User.FullName
                    != fullName
                ||
                profileRecord.User.PhoneNumber
                    != phoneNumber
                ||
                profileRecord.User.AvatarUrl
                    != avatarUrl;

            if (!hasChanges)
            {
                return ServiceResult.Failure(
                    "No profile changes were detected.");
            }

            profileRecord.User.FullName =
                fullName;

            profileRecord.User.PhoneNumber =
                phoneNumber;

            profileRecord.User.AvatarUrl =
                avatarUrl;

            profileRecord.User.UpdatedAt =
                DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                return ServiceResult.Success(
                    "Your profile was updated successfully.");
            }
            catch (DbUpdateException)
            {
                return ServiceResult.Failure(
                    "The profile could not be updated "
                    + "because the database rejected the values.");
            }
        }

        private static bool IsValidAvatarUrl(
            string avatarUrl)
        {
            if (avatarUrl.StartsWith("/"))
            {
                return true;
            }

            bool isValidUrl =
                Uri.TryCreate(
                    avatarUrl,
                    UriKind.Absolute,
                    out Uri? uri);

            if (!isValidUrl
                || uri == null)
            {
                return false;
            }

            return uri.Scheme == Uri.UriSchemeHttp
                   || uri.Scheme == Uri.UriSchemeHttps;
        }
    }
}