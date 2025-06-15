using System.Threading.Tasks;

namespace cookie_authentication_authorization_demo.Services;

public interface IAuditLogService
{
    Task LogActionAsync(string entity, string action, string userId, string details);
} 