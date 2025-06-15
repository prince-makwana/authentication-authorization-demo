namespace cookie_authentication_authorization_demo.Enums;

/// <summary>
/// Enum representing the possible statuses of an order
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order has been created but not yet processed
    /// </summary>
    Pending,

    /// <summary>
    /// Order has been confirmed
    /// </summary>
    Confirmed,

    /// <summary>
    /// Order is being processed
    /// </summary>
    Processing,

    /// <summary>
    /// Order has been shipped
    /// </summary>
    Shipped,

    /// <summary>
    /// Order has been delivered
    /// </summary>
    Delivered,

    /// <summary>
    /// Order has been cancelled
    /// </summary>
    Cancelled,

    /// <summary>
    /// Order has been refunded
    /// </summary>
    Refunded
} 