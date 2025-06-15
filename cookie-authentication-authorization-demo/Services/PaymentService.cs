using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace cookie_authentication_authorization_demo.Services
{
    /// <summary>
    /// Service for managing payments
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ApplicationDbContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            payment.Status = PaymentStatus.Pending.ToString();
            payment.CreatedAt = DateTime.UtcNow;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.Status == status.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByPaymentMethodAsync(string paymentMethod)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.PaymentMethod == paymentMethod)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByTransactionIdAsync(string transactionId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.TransactionId == transactionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAndStatusAsync(string userId, PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.UserId == userId && p.Status == status.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderAndStatusAsync(int orderId, PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.OrderId == orderId && p.Status == status.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAndStatusAsync(DateTime startDate, DateTime endDate, PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.Status == status.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByAmountRangeAndStatusAsync(decimal minAmount, decimal maxAmount, PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount && p.Status == status.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByPaymentMethodAndStatusAsync(string paymentMethod, PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.PaymentMethod == paymentMethod && p.Status == status.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByTransactionIdAndStatusAsync(string transactionId, PaymentStatus status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.TransactionId == transactionId && p.Status == status.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.UserId == userId && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderAndDateRangeAsync(int orderId, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.OrderId == orderId && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByAmountRangeAndDateRangeAsync(decimal minAmount, decimal maxAmount, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByPaymentMethodAndDateRangeAsync(string paymentMethod, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.PaymentMethod == paymentMethod && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByTransactionIdAndDateRangeAsync(string transactionId, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.TransactionId == transactionId && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserStatusAndDateRangeAsync(string userId, PaymentStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.UserId == userId && p.Status == status.ToString() && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderStatusAndDateRangeAsync(int orderId, PaymentStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.OrderId == orderId && p.Status == status.ToString() && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByAmountRangeStatusAndDateRangeAsync(decimal minAmount, decimal maxAmount, PaymentStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount && p.Status == status.ToString() && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByPaymentMethodStatusAndDateRangeAsync(string paymentMethod, PaymentStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.PaymentMethod == paymentMethod && p.Status == status.ToString() && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByTransactionIdStatusAndDateRangeAsync(string transactionId, PaymentStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.TransactionId == transactionId && p.Status == status.ToString() && p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatusAsync(int id, string status)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return false;
            }

            if (payment.Status == PaymentStatus.Completed.ToString())
            {
                return false;
            }

            payment.Status = status;
            payment.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Payment> ProcessPaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {id} not found");

            payment.Status = PaymentStatus.Completed.ToString();
            payment.ProcessedAt = DateTime.UtcNow;
            payment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> RefundPaymentAsync(int id, decimal amount)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {id} not found");

            if (amount > payment.Amount)
                throw new InvalidOperationException("Refund amount cannot exceed payment amount");

            payment.Status = PaymentStatus.Refunded.ToString();
            payment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> CancelPaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return false;
            }

            if (payment.Status == PaymentStatus.Completed.ToString() || 
                payment.Status == PaymentStatus.Cancelled.ToString())
            {
                return false;
            }

            payment.Status = PaymentStatus.Cancelled.ToString();
            payment.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> VerifyPaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return false;
            }

            // Here you would typically verify the payment with the payment gateway
            // For demo purposes, we'll just return true if the payment is completed
            return payment.Status == PaymentStatus.Completed.ToString();
        }
    }
} 