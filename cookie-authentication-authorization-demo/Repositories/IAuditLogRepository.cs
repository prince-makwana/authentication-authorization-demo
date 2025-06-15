using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public interface IAuditLogRepository
{
    Task<AuditLog> GetByIdAsync(int id);
    Task<IEnumerable<AuditLog>> GetAllAsync();
    Task<AuditLog> AddAsync(AuditLog log);
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId);
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName);
} 