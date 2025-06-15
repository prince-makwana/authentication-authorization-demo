using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId);
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment> ProcessPaymentAsync(int id);
        Task<Payment> RefundPaymentAsync(int id, decimal amount);
        Task<bool> CancelPaymentAsync(int id);
        Task<bool> VerifyPaymentAsync(int id);
    }
} 