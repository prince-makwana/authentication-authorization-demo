using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using System.Security.Claims;
using System.Text.Json;
using cookie_authentication_authorization_demo.Services;

namespace cookie_authentication_authorization_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly IAuditService _auditService;

        public PaymentsController(ApplicationDbContext context, IPaymentService paymentService, IAuditService auditService)
        {
            _context = context;
            _paymentService = paymentService;
            _auditService = auditService;
        }

        // GET: api/payments
        [HttpGet]
        [Authorize(Policy = "RequireFinanceTeamRole")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            if (payment.UserId != userId && !User.IsInRole("Administrator") && !User.IsInRole("FinanceTeam"))
            {
                return Forbid();
            }

            return payment;
        }

        // POST: api/payments
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePayment(CreatePaymentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payment = new Payment
            {
                OrderId = request.OrderId,
                UserId = userId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                Status = PaymentStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "Payment",
                "Create",
                userId,
                JsonSerializer.Serialize(new { payment.Id, payment.OrderId, payment.Amount, payment.PaymentMethod })
            );

            return Ok(payment);
        }

        // POST: api/payments/5/refund
        [HttpPost("{id}/refund")]
        [Authorize(Policy = "RequireFinanceTeamRole")]
        public async Task<IActionResult> RefundPayment(int id, RefundRequest request)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            if (payment.Status != PaymentStatus.Completed.ToString())
            {
                return BadRequest("Only completed payments can be refunded");
            }

            payment.Status = PaymentStatus.Refunded.ToString();
            payment.UpdatedAt = DateTime.UtcNow;

            // Update order status
            payment.Order.Status = OrderStatus.Refunded;
            payment.Order.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await _auditService.LogActionAsync(
                "Payment",
                "Refund",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                JsonSerializer.Serialize(new { paymentId = id, oldStatus = payment.Status, newStatus = PaymentStatus.Refunded })
            );

            return Ok(new { 
                PaymentId = payment.Id,
                Status = payment.Status,
                UpdatedAt = payment.UpdatedAt
            });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Administrator,FinanceTeam")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] string status)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            if (payment.Status == PaymentStatus.Completed.ToString())
            {
                return BadRequest(new { message = "Cannot update completed payment" });
            }

            var oldStatus = payment.Status;
            payment.Status = status;
            payment.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                await _auditService.LogActionAsync(
                    "Payment",
                    "UpdateStatus",
                    userId,
                    JsonSerializer.Serialize(new { paymentId = id, oldStatus, newStatus = status })
                );

                return Ok(new { message = "Payment status updated successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByOrder(int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payments = await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();

            if (!payments.Any())
            {
                return NotFound();
            }

            if (payments.First().UserId != userId && !User.IsInRole("Administrator") && !User.IsInRole("FinanceTeam"))
            {
                return Forbid();
            }

            return payments;
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }

        private string GeneratePaymentNumber()
        {
            return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }

    public class CreatePaymentRequest
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public decimal Amount { get; set; }
    }

    public class RefundRequest
    {
        public string Reason { get; set; } = null!;
    }
} 