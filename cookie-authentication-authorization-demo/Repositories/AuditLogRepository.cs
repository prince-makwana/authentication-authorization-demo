using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly ApplicationDbContext _context;

    public AuditLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AuditLog> GetByIdAsync(int id)
    {
        return await _context.AuditLogs.FindAsync(id);
    }

    public async Task<IEnumerable<AuditLog>> GetAllAsync()
    {
        return await _context.AuditLogs.ToListAsync();
    }

    public async Task<AuditLog> AddAsync(AuditLog log)
    {
        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId)
    {
        return await _context.AuditLogs.Where(a => a.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName)
    {
        return await _context.AuditLogs.Where(a => a.EntityName == entityName).ToListAsync();
    }
} 