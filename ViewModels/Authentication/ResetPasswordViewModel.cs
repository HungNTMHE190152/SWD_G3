using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Authentication;

public class ResetPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Otp { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword),
        ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = "";
}