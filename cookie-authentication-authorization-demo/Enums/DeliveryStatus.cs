namespace cookie_authentication_authorization_demo.Enums;

/// <summary>
/// Enum representing the possible statuses of a delivery
/// </summary>
public enum DeliveryStatus
{
    /// <summary>
    /// Delivery is pending and not yet started
    /// </summary>
    Pending,

    /// <summary>
    /// Delivery is in progress
    /// </summary>
    InProgress,

    /// <summary>
    /// Delivery is out for delivery
    /// </summary>
    OutForDelivery,

    /// <summary>
    /// Delivery is in transit
    /// </summary>
    InTransit,

    /// <summary>
    /// Delivery has been completed successfully
    /// </summary>
    Delivered,

    /// <summary>
    /// Delivery failed to complete
    /// </summary>
    Failed,

    /// <summary>
    /// Delivery was cancelled
    /// </summary>
    Cancelled
} 