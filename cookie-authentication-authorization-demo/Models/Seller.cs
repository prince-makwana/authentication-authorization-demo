using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace cookie_authentication_authorization_demo.Models;

/// <summary>
/// Represents a seller in the e-commerce system
/// </summary>
public class Seller
{
    /// <summary>
    /// Unique identifier for the seller
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Business name of the seller
    /// Required field with maximum length of 100 characters
    /// </summary>
    [Required]
    [StringLength(100)]
    public string BusinessName { get; set; } = null!;

    /// <summary>
    /// Email address of the seller
    /// Required field with maximum length of 100 characters
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Phone number of the seller
    /// Required field with maximum length of 20 characters
    /// </summary>
    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Business address of the seller
    /// Required field with maximum length of 200 characters
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Address { get; set; } = null!;

    /// <summary>
    /// Seller's tax identification number.
    /// </summary>
    [StringLength(50)]
    public string TaxId { get; set; } = null!;

    /// <summary>
    /// Seller's business registration number.
    /// </summary>
    [StringLength(50)]
    public string RegistrationNumber { get; set; } = null!;

    /// <summary>
    /// Rating of the seller (0-5)
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// Date and time when the seller was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the seller was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Whether the seller is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Collection of products sold by this seller
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
} 