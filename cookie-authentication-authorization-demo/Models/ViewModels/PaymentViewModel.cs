using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cookie_authentication_authorization_demo.Models.ViewModels;

/// <summary>
/// View Model for displaying payment information
/// </summary>
public class PaymentViewModel
{
    /// <summary>
    /// Unique identifier for the payment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the user who made the payment
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Name of the user who made the payment
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// ID of the order this payment is associated with
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Amount of the payment
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Current status of the payment
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Payment method used
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the payment was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the payment was processed
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Date and time when the payment was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 