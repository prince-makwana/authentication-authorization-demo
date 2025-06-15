using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Services;

public interface IPaymentService
{
    Task<PaymentDTO> CreatePaymentAsync(string userId, CreatePaymentViewModel model);
    Task<PaymentDTO?> GetPaymentByIdAsync(int paymentId);
    Task<IEnumerable<PaymentDTO>> GetUserPaymentsAsync(string userId);
    Task<PaymentDTO> UpdatePaymentStatusAsync(int paymentId, PaymentStatus newStatus, string userId);
    Task<PaymentDTO> ProcessPaymentAsync(int paymentId, string userId);
    Task<PaymentDTO> CompletePaymentAsync(int paymentId, string userId);
    Task<PaymentDTO> CancelPaymentAsync(int paymentId, string userId);
    Task<PaymentDTO> RefundPaymentAsync(int paymentId, string userId);
    Task<bool> VerifyPaymentAsync(int paymentId);
} 