using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;
using cookie_authentication_authorization_demo.Enums;
using cookie_authentication_authorization_demo.Mappers;
using cookie_authentication_authorization_demo.Repositories;

namespace cookie_authentication_authorization_demo.Services;

/// <summary>
/// Service for managing payments
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;
    private readonly IAuditService _auditService;

    public PaymentService(
        IPaymentRepository paymentRepository,
        ILogger<PaymentService> logger,
        IAuditService auditService)
    {
        _paymentRepository = paymentRepository;
        _logger = logger;
        _auditService = auditService;
    }

    public async Task<PaymentDTO> CreatePaymentAsync(string userId, CreatePaymentViewModel model)
    {
        try
        {
            var payment = new Payment
            {
                UserId = userId,
                OrderId = model.OrderId,
                Amount = model.Amount,
                PaymentMethod = model.PaymentMethod,
                Status = PaymentStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            payment = await _paymentRepository.AddAsync(payment);

            await _auditService.LogActionAsync(
                "Payment",
                "Create",
                userId,
                System.Text.Json.JsonSerializer.Serialize(new { paymentId = payment.Id, amount = payment.Amount })
            );

            return PaymentMapper.ToDTO(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for user {UserId}", userId);
            throw;
        }
    }

    public async Task<PaymentDTO?> GetPaymentByIdAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdWithUserAsync(id);
        if (payment == null)
        {
            return null;
        }
        return PaymentMapper.ToDTO(payment);
    }

    public async Task<IEnumerable<PaymentDTO>> GetUserPaymentsAsync(string userId)
    {
        var payments = await _paymentRepository.GetByUserIdAsync(userId);
        return payments.Select(p => PaymentMapper.ToDTO(p));
    }

    public async Task<PaymentDTO> UpdatePaymentStatusAsync(int id, PaymentStatus newStatus, string userId)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found");
        }

        var oldStatus = payment.Status;
        payment.Status = newStatus.ToString();
        payment.UpdatedAt = DateTime.UtcNow;

        payment = await _paymentRepository.UpdateAsync(payment);

        await _auditService.LogActionAsync(
            "Payment",
            "UpdateStatus",
            userId,
            System.Text.Json.JsonSerializer.Serialize(new { paymentId = payment.Id, oldStatus, newStatus = payment.Status })
        );

        return PaymentMapper.ToDTO(payment);
    }

    public async Task<PaymentDTO> ProcessPaymentAsync(int id, string userId)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found");
        }

        if (payment.Status != PaymentStatus.Pending.ToString())
        {
            throw new InvalidOperationException($"Cannot process payment with status {payment.Status}");
        }

        payment.Status = PaymentStatus.Processing.ToString();
        payment.UpdatedAt = DateTime.UtcNow;

        payment = await _paymentRepository.UpdateAsync(payment);

        await _auditService.LogActionAsync(
            "Payment",
            "Process",
            userId,
            System.Text.Json.JsonSerializer.Serialize(new { paymentId = payment.Id, status = payment.Status })
        );

        return PaymentMapper.ToDTO(payment);
    }

    public async Task<PaymentDTO> CompletePaymentAsync(int id, string userId)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found");
        }

        if (payment.Status != PaymentStatus.Processing.ToString())
        {
            throw new InvalidOperationException($"Cannot complete payment with status {payment.Status}");
        }

        payment.Status = PaymentStatus.Completed.ToString();
        payment.ProcessedAt = DateTime.UtcNow;
        payment.UpdatedAt = DateTime.UtcNow;

        payment = await _paymentRepository.UpdateAsync(payment);

        await _auditService.LogActionAsync(
            "Payment",
            "Complete",
            userId,
            System.Text.Json.JsonSerializer.Serialize(new { paymentId = payment.Id, status = payment.Status })
        );

        return PaymentMapper.ToDTO(payment);
    }

    public async Task<PaymentDTO> CancelPaymentAsync(int id, string userId)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found");
        }

        if (payment.Status != PaymentStatus.Pending.ToString() && payment.Status != PaymentStatus.Processing.ToString())
        {
            throw new InvalidOperationException($"Cannot cancel payment with status {payment.Status}");
        }

        payment.Status = PaymentStatus.Cancelled.ToString();
        payment.UpdatedAt = DateTime.UtcNow;

        payment = await _paymentRepository.UpdateAsync(payment);

        await _auditService.LogActionAsync(
            "Payment",
            "Cancel",
            userId,
            System.Text.Json.JsonSerializer.Serialize(new { paymentId = payment.Id, status = payment.Status })
        );

        return PaymentMapper.ToDTO(payment);
    }

    public async Task<PaymentDTO> RefundPaymentAsync(int id, string userId)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found");
        }

        if (payment.Status != PaymentStatus.Completed.ToString())
        {
            throw new InvalidOperationException($"Cannot refund payment with status {payment.Status}");
        }

        payment.Status = PaymentStatus.Refunded.ToString();
        payment.UpdatedAt = DateTime.UtcNow;

        payment = await _paymentRepository.UpdateAsync(payment);

        await _auditService.LogActionAsync(
            "Payment",
            "Refund",
            userId,
            System.Text.Json.JsonSerializer.Serialize(new { paymentId = payment.Id, status = payment.Status })
        );

        return PaymentMapper.ToDTO(payment);
    }

    public async Task<bool> VerifyPaymentAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found");
        }

        return payment.Status == PaymentStatus.Completed.ToString();
    }
} 