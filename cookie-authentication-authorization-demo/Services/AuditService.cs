using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using Microsoft.EntityFrameworkCore;

namespace cookie_authentication_authorization_demo.Services;

/// <summary>
/// Service responsible for handling audit logging across the application
/// </summary>
public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogActionAsync(string entityName, string action, string? userId, string details)
    {
        var auditLog = new AuditLog
        {
            EntityName = entityName,
            Action = action,
            UserId = userId,
            Details = details,
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }
} 