using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Profile
{
    public class EditProfileViewModel
        : IValidatableObject
    {
        [Display(Name = "Full name")]
        [Required(
            ErrorMessage =
                "Full name is required.")]
        [StringLength(
            150,
            MinimumLength = 2,
            ErrorMessage =
                "Full name must contain between 2 and 150 characters.")]
        public string FullName { get; set; } =
            string.Empty;

        [Display(Name = "Phone number")]
        [StringLength(
            30,
            ErrorMessage =
                "Phone number cannot exceed 30 characters.")]
        [RegularExpression(
            @"^[0-9+\-\s()]*$",
            ErrorMessage =
                "Phone number contains invalid characters.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Avatar URL")]
        [StringLength(
            500,
            ErrorMessage =
                "Avatar URL cannot exceed 500 characters.")]
        public string? AvatarUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(AvatarUrl))
            {
                yield break;
            }

            string avatarValue =
                AvatarUrl.Trim();

            bool isLocalPath =
                avatarValue.StartsWith("/");

            bool isWebUrl =
                Uri.TryCreate(
                    avatarValue,
                    UriKind.Absolute,
                    out Uri? avatarUri)
                &&
                (
                    avatarUri.Scheme == Uri.UriSchemeHttp
                    || avatarUri.Scheme == Uri.UriSchemeHttps
                );

            if (!isLocalPath
                && !isWebUrl)
            {
                yield return new ValidationResult(
                    "Avatar URL must be an HTTP/HTTPS URL "
                    + "or a local path beginning with '/'.",
                    new[]
                    {
                        nameof(AvatarUrl)
                    });
            }
        }
    }
}