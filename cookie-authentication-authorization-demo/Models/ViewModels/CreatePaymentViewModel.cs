using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cookie_authentication_authorization_demo.Models.ViewModels;

/// <summary>
/// View Model for creating a new payment
/// </summary>
public class CreatePaymentViewModel
{
    /// <summary>
    /// ID of the order this payment is associated with
    /// Required field
    /// </summary>
    [Required]
    public int OrderId { get; set; }

    /// <summary>
    /// Amount of the payment
    /// Required field with minimum value of 0.01
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment method to use
    /// Required field
    /// </summary>
    [Required]
    public string PaymentMethod { get; set; } = string.Empty;
} 