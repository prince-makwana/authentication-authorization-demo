using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models.ViewModels;

/// <summary>
/// View Model for displaying order information
/// </summary>
public class OrderViewModel
{
    /// <summary>
    /// Unique identifier for the order
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the user who placed the order
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Name of the user who placed the order
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Total amount of the order
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Current status of the order
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Shipping address for the order
    /// </summary>
    public string ShippingAddress { get; set; } = string.Empty;

    /// <summary>
    /// Contact phone number for the order
    /// </summary>
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
    public ICollection<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
} 