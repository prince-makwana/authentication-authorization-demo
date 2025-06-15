using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Services;

/// <summary>
/// Interface for audit logging service
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Logs a user action with detailed information
    /// </summary>
    /// <param name="entityType">Type of the entity being acted upon (e.g., "Product", "Order")</param>
    /// <param name="action">Type of action performed (e.g., "Create", "Update", "Delete")</param>
    /// <param name="userId">ID of the user performing the action</param>
    /// <param name="details">Additional details about the action</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task LogActionAsync(string entityType, string action, string? userId, string details);
} 