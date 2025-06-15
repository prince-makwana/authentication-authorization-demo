using System.ComponentModel.DataAnnotations;

namespace cookie_authentication_authorization_demo.Models.ViewModels;

/// <summary>
/// View Model for creating a new order
/// </summary>
public class CreateOrderViewModel
{
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
    [Phone]
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// Collection of order items
    /// Required field with at least one item
    /// </summary>
    [Required]
    [MinLength(1)]
    public ICollection<CreateOrderItemViewModel> OrderItems { get; set; } = new List<CreateOrderItemViewModel>();
}

/// <summary>
/// View Model for creating a new order item
/// </summary>
public class CreateOrderItemViewModel
{
    /// <summary>
    /// ID of the product to order
    /// Required field
    /// </summary>
    [Required]
    public int ProductId { get; set; }

    /// <summary>
    /// Quantity of the product to order
    /// Required field with minimum value of 1
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product
    /// Required field with minimum value of 0.01
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
} 