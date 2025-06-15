using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cookie_authentication_authorization_demo.Models;

/// <summary>
/// Represents an item in an order
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Unique identifier for the order item
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// ID of the order this item belongs to
    /// Required field
    /// </summary>
    [Required]
    public int OrderId { get; set; }

    /// <summary>
    /// Navigation property for the order this item belongs to
    /// </summary>
    public Order Order { get; set; } = null!;

    /// <summary>
    /// ID of the product in this order item
    /// Required field
    /// </summary>
    [Required]
    public int ProductId { get; set; }

    /// <summary>
    /// Navigation property for the product in this order item
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Quantity of the product ordered
    /// Required field with minimum value of 1
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product at the time of order
    /// Required field with precision of 18 digits and 2 decimal places
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Date and time when the order item was created
    /// Automatically set to current UTC time when order item is created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 