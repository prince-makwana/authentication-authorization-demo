namespace cookie_authentication_authorization_demo.Models.ViewModels;

/// <summary>
/// View Model for displaying order item information
/// </summary>
public class OrderItemViewModel
{
    /// <summary>
    /// Unique identifier for the order item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the order this item belongs to
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// ID of the product in this order item
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Name of the product
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the product ordered
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product at the time of order
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Total price for this order item (Quantity * UnitPrice)
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// URL of the product image
    /// </summary>
    public string? ProductImageUrl { get; set; }
} 