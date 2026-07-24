using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Authentication;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}