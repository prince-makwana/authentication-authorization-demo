using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAuditTeamRole")]
    public class AuditController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/audit/orders
        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrderAudit(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt <= endDate.Value);
            }

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.TotalAmount,
                    o.Status,
                    CustomerName = o.User.UserName,
                    CustomerEmail = o.User.Email,
                    o.CreatedAt,
                    o.UpdatedAt
                })
                .ToListAsync();

            return orders;
        }

        // GET: api/audit/transactions
        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<object>>> GetTransactionAudit(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.User)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= endDate.Value);
            }

            var transactions = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.PaymentNumber,
                    p.Amount,
                    p.Status,
                    p.PaymentMethod,
                    p.TransactionId,
                    OrderNumber = p.Order.OrderNumber,
                    CustomerName = p.Order.User.UserName,
                    CustomerEmail = p.Order.User.Email,
                    p.CreatedAt,
                    p.UpdatedAt
                })
                .ToListAsync();

            return transactions;
        }

        // GET: api/audit/inventory
        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<object>>> GetInventoryAudit(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = _context.Products
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= startDate.Value || 
                    (p.UpdatedAt.HasValue && p.UpdatedAt.Value >= startDate.Value));
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= endDate.Value || 
                    (p.UpdatedAt.HasValue && p.UpdatedAt.Value <= endDate.Value));
            }

            var inventory = await query
                .OrderByDescending(p => p.UpdatedAt ?? p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.StockQuantity,
                    p.Category,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.IsActive
                })
                .ToListAsync();

            return inventory;
        }
    }
} 