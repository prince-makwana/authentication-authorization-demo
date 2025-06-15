using System.ComponentModel.DataAnnotations;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models.ViewModels;

/// <summary>
/// View Model for Product information
/// </summary>
public class ProductViewModel
{
    /// <summary>
    /// Unique identifier for the product
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the product
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Current stock quantity of the product
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Category of the product
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// URL of the product image
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// ID of the seller who owns this product
    /// </summary>
    public int SellerId { get; set; }

    /// <summary>
    /// Name of the seller who owns this product
    /// </summary>
    public string SellerName { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the product
    /// </summary>
    public ProductStatus Status { get; set; }

    /// <summary>
    /// Whether the product is currently active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date and time when the product was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the product was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 