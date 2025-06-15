using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cookie_authentication_authorization_demo.Models;

/// <summary>
/// Represents a payment in the e-commerce system
/// </summary>
public class Payment
{
    /// <summary>
    /// Unique identifier for the payment
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Unique payment number for tracking and reference
    /// Required field with maximum length of 20 characters
    /// </summary>
    [Required]
    [StringLength(20)]
    public string PaymentNumber { get; set; } = null!;

    /// <summary>
    /// ID of the order this payment is associated with
    /// Required field
    /// </summary>
    [Required]
    public int OrderId { get; set; }

    /// <summary>
    /// Navigation property for the order this payment is associated with
    /// </summary>
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    /// <summary>
    /// ID of the user who made the payment
    /// Required field
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property for the user who made the payment
    /// </summary>
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    /// <summary>
    /// Amount of the payment
    /// Required field with precision of 18 digits and 2 decimal places
    /// </summary>
    [Required]
    [Range(0.01, 999999999999999999.99)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Method used for payment (e.g., CreditCard, PayPal, BankTransfer)
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the payment
    /// Required field
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Transaction ID from the payment processor
    /// Optional field with maximum length of 100 characters
    /// </summary>
    [StringLength(100)]
    public string? TransactionId { get; set; }

    /// <summary>
    /// Date and time when the payment was made
    /// Automatically set to current UTC time when payment is created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the payment was last updated
    /// Automatically updated when payment status changes
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Date and time when the payment was processed
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
}

/// <summary>
/// Enum representing the possible statuses of a payment
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment has been initiated but not yet processed
    /// </summary>
    Pending,

    /// <summary>
    /// Payment has been successfully processed
    /// </summary>
    Completed,

    /// <summary>
    /// Payment has failed
    /// </summary>
    Failed,

    /// <summary>
    /// Payment has been refunded
    /// </summary>
    Refunded,

    /// <summary>
    /// Payment has been cancelled
    /// </summary>
    Cancelled
} 