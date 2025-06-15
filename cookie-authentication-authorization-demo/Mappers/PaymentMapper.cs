using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;

namespace cookie_authentication_authorization_demo.Mappers;

public static class PaymentMapper
{
    public static PaymentDTO? ToDTO(Payment? payment)
    {
        if (payment == null)
        {
            return null;
        }

        return new PaymentDTO
        {
            Id = payment.Id,
            UserId = payment.UserId,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            ProcessedAt = payment.ProcessedAt,
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt,
            UserName = payment.User?.UserName
        };
    }

    public static PaymentViewModel ToViewModel(Payment payment)
    {
        return new PaymentViewModel
        {
            Id = payment.Id,
            UserId = payment.UserId,
            UserName = payment.User?.UserName ?? string.Empty,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            Status = payment.Status,
            PaymentMethod = payment.PaymentMethod,
            CreatedAt = payment.CreatedAt,
            ProcessedAt = payment.ProcessedAt,
            UpdatedAt = payment.UpdatedAt
        };
    }
} 