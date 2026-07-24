using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Authentication;

public class VerifyOtpViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [Display(Name = "OTP Code")]
    public string Otp { get; set; } = "";
}