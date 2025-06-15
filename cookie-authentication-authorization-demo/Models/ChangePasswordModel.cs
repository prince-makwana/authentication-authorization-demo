using System.ComponentModel.DataAnnotations;

namespace cookie_authentication_authorization_demo.Models;

public class ChangePasswordModel
{
    [Required]
    [StringLength(100)]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = null!;
} 