using System.ComponentModel.DataAnnotations;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models.ViewModels;

/// <summary>
/// View Model for updating an existing product
/// </summary>
public class UpdateProductViewModel
{
    /// <summary>
    /// Name of the product
    /// Required field with maximum length of 100 characters
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the product
    /// Required field
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Price of the product
    /// Required field with precision of 18 digits and 2 decimal places
    /// </summary>
    [Required]
    [Range(0.01, 999999999999999999.99)]
    public decimal Price { get; set; }

    /// <summary>
    /// Current stock quantity of the product
    /// Required field
    /// </summary>
    [Required]
    public int StockQuantity { get; set; }

    /// <summary>
    /// Category of the product
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// URL of the product image
    /// Optional field
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// ID of the seller who owns this product
    /// Required field
    /// </summary>
    [Required]
    public int SellerId { get; set; }

    /// <summary>
    /// Whether the product is currently active
    /// </summary>
    public bool IsActive { get; set; }
} 