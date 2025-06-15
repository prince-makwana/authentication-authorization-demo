namespace cookie_authentication_authorization_demo.Enums;

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
    /// Payment is being processed
    /// </summary>
    Processing,

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