using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Authentication;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 4)]
    public string Username { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password),
        ErrorMessage = "Password confirmation does not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = "";
}