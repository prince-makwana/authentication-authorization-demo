using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cookie_authentication_authorization_demo.Models.DTOs;

/// <summary>
/// Data Transfer Object for Order Item information
/// </summary>
public class OrderItemDTO
{
    /// <summary>
    /// Unique identifier for the order item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the order this item belongs to
    /// Required field
    /// </summary>
    [Required]
    public int OrderId { get; set; }

    /// <summary>
    /// ID of the product in this order item
    /// Required field
    /// </summary>
    [Required]
    public int ProductId { get; set; }

    /// <summary>
    /// Quantity of the product ordered
    /// Required field
    /// </summary>
    [Required]
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product at the time of order
    /// Required field with precision of 18 digits and 2 decimal places
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Total price for this order item (Quantity * UnitPrice)
    /// Required field with precision of 18 digits and 2 decimal places
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Name of the product at the time of order
    /// Required field
    /// </summary>
    [Required]
    public string ProductName { get; set; } = string.Empty;
} 