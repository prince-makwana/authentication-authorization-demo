using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireDeliveryTeamRole")]
    public class DeliveryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeliveryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/delivery/orders
        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrdersToDeliver()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Where(o => o.Status == OrderStatus.Processing || o.Status == OrderStatus.Shipped)
                .OrderBy(o => o.CreatedAt)
                .Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.ShippingAddress,
                    o.Status,
                    CustomerName = o.User.UserName,
                    CustomerEmail = o.User.Email,
                    o.CreatedAt,
                    o.UpdatedAt
                })
                .ToListAsync();
        }

        // POST: api/delivery/5/deliver
        [HttpPost("{id}/deliver")]
        public async Task<IActionResult> MarkAsDelivered(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.Status != OrderStatus.Processing && order.Status != OrderStatus.Shipped)
            {
                return BadRequest("Order is not in a state that can be delivered");
            }

            order.Status = OrderStatus.Delivered;
            order.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { 
                OrderId = order.Id,
                Status = order.Status,
                UpdatedAt = order.UpdatedAt
            });
        }

        // POST: api/delivery/5/reportIssue
        [HttpPost("{id}/reportIssue")]
        public async Task<IActionResult> ReportDeliveryIssue(int id, DeliveryIssueRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.Status != OrderStatus.Processing && order.Status != OrderStatus.Shipped)
            {
                return BadRequest("Cannot report issues for orders that are not in delivery");
            }

            // Update order status and add delivery notes
            order.Status = OrderStatus.Cancelled; // Using Cancelled status for delivery issues
            order.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { 
                OrderId = order.Id,
                Status = order.Status,
                UpdatedAt = order.UpdatedAt
            });
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }

    public class DeliveryIssueRequest
    {
        public string IssueDescription { get; set; } = null!;
    }
} 