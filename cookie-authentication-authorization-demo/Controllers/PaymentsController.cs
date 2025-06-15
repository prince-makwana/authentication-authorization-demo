using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cookie_authentication_authorization_demo.Models.ViewModels;
using cookie_authentication_authorization_demo.Services;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(
        IPaymentService paymentService,
        ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentViewModel model)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payment = await _paymentService.CreatePaymentAsync(userId, model);
            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");
            return StatusCode(500, "An error occurred while creating the payment");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayment(int id)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst("sub")?.Value;
            if (payment.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Ok(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment {PaymentId}", id);
            return StatusCode(500, "An error occurred while retrieving the payment");
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserPayments()
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payments = await _paymentService.GetUserPaymentsAsync(userId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user payments");
            return StatusCode(500, "An error occurred while retrieving the payments");
        }
    }

    [HttpPost("{id}/process")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProcessPayment(int id)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            var payment = await _paymentService.ProcessPaymentAsync(id, userId);
            return Ok(payment);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment {PaymentId}", id);
            return StatusCode(500, "An error occurred while processing the payment");
        }
    }

    [HttpPost("{id}/complete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CompletePayment(int id)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            var payment = await _paymentService.CompletePaymentAsync(id, userId);
            return Ok(payment);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing payment {PaymentId}", id);
            return StatusCode(500, "An error occurred while completing the payment");
        }
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelPayment(int id)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            
            if (payment == null)
            {
                return NotFound();
            }

            if (payment.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            payment = await _paymentService.CancelPaymentAsync(id, userId);
            return Ok(payment);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling payment {PaymentId}", id);
            return StatusCode(500, "An error occurred while canceling the payment");
        }
    }

    [HttpPost("{id}/refund")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RefundPayment(int id)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            var payment = await _paymentService.RefundPaymentAsync(id, userId);
            return Ok(payment);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refunding payment {PaymentId}", id);
            return StatusCode(500, "An error occurred while refunding the payment");
        }
    }

    [HttpGet("{id}/verify")]
    public async Task<IActionResult> VerifyPayment(int id)
    {
        try
        {
            var isVerified = await _paymentService.VerifyPaymentAsync(id);
            return Ok(new { isVerified });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying payment {PaymentId}", id);
            return StatusCode(500, "An error occurred while verifying the payment");
        }
    }
} 