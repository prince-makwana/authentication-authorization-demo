using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cookie_authentication_authorization_demo.Models;

/// <summary>
/// Represents an audit log entry in the system
/// Used to track all important system activities for security and compliance
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Unique identifier for the audit log entry
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Name of the entity being audited (e.g., "Product", "Order", "Payment")
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// Type of action performed (e.g., Create, Update, Delete, View)
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// ID of the user who performed the action
    /// Required field with maximum length of 50 characters
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// ID of the entity being audited
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string EntityId { get; set; } = string.Empty;

    /// <summary>
    /// Additional details about the action
    /// Required field
    /// </summary>
    [Required]
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// IP address of the user who performed the action
    /// Required field with maximum length of 50 characters
    /// </summary>
    [Required]
    [StringLength(50)]
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the action was performed
    /// Automatically set to current UTC time when audit log is created
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Navigation property for the user who performed the action
    /// </summary>
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }
} 