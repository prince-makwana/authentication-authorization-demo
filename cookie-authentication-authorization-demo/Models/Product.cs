using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models;

/// <summary>
/// Represents a product in the e-commerce system
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier for the product
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Name of the product
    /// Required field with maximum length of 100 characters
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of the product
    /// Required field with maximum length of 500 characters
    /// </summary>
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Price of the product
    /// Required field with precision of 18 digits and 2 decimal places
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Current stock quantity of the product
    /// Required field with minimum value of 0
    /// </summary>
    [Required]
    public int StockQuantity { get; set; }

    /// <summary>
    /// Category of the product (e.g., Electronics, Clothing, Books)
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the product
    /// </summary>
    [Required]
    public ProductStatus Status { get; set; } = ProductStatus.InStock;

    /// <summary>
    /// URL of the product's image
    /// Optional field with maximum length of 200 characters
    /// </summary>
    [StringLength(200)]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Date and time when the product was created
    /// Automatically set to current UTC time when product is created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the product was last updated
    /// Automatically updated when product is modified
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Whether the product is active in the system
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Collection of order items containing this product
    /// </summary>
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Foreign key for the seller of this product
    /// </summary>
    [Required]
    public int SellerId { get; set; }

    /// <summary>
    /// Navigation property for the seller of this product
    /// </summary>
    public Seller Seller { get; set; } = null!;
} 