using System.ComponentModel.DataAnnotations;

namespace cookie_authentication_authorization_demo.Models;

/// <summary>
/// Model for updating user profile information
/// </summary>
public class UpdateProfileModel
{
    /// <summary>
    /// User's email address
    /// Required field with email validation
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// User's first name
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
} 