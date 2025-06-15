namespace cookie_authentication_authorization_demo.Models;

/// <summary>
/// Enum representing the possible statuses of a product
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// Product is in stock and available for purchase
    /// </summary>
    InStock,

    /// <summary>
    /// Product is out of stock
    /// </summary>
    OutOfStock,

    /// <summary>
    /// Product is discontinued
    /// </summary>
    Discontinued,

    /// <summary>
    /// Product is temporarily unavailable
    /// </summary>
    Unavailable
} 