using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models.DTOs;

/// <summary>
/// Data Transfer Object for Order information
/// </summary>
public class OrderDTO
{
    /// <summary>
    /// Unique identifier for the order
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the user who placed the order
    /// Required field
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Total amount of the order
    /// Required field with precision of 18 digits and 2 decimal places
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Current status of the order
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Shipping address for the order
    /// Required field
    /// </summary>
    [Required]
    public string ShippingAddress { get; set; } = string.Empty;

    /// <summary>
    /// Contact phone number for the order
    /// Required field
    /// </summary>
    [Required]
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the order was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the order was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Collection of order items
    /// </summary>
    public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
} 