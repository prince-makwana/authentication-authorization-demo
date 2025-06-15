using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cookie_authentication_authorization_demo.Services;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;
using cookie_authentication_authorization_demo.Enums;
using System.Security.Claims;
using System.Text.Json;
using cookie_authentication_authorization_demo.Data;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Services;

namespace cookie_authentication_authorization_demo.Controllers
{
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
        public async Task<ActionResult<PaymentDTO>> CreatePayment(CreatePaymentViewModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        public async Task<ActionResult<PaymentDTO>> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || payment.UserId != userId)
                {
                    return Forbid();
                }

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment {PaymentId}", id);
                return StatusCode(500, "An error occurred while retrieving the payment");
            }
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetUserPayments()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        public async Task<ActionResult<PaymentDTO>> ProcessPayment(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

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
        public async Task<ActionResult<PaymentDTO>> CompletePayment(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

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
        public async Task<ActionResult<PaymentDTO>> CancelPayment(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var payment = await _paymentService.CancelPaymentAsync(id, userId);
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
                _logger.LogError(ex, "Error cancelling payment {PaymentId}", id);
                return StatusCode(500, "An error occurred while cancelling the payment");
            }
        }

        [HttpPost("{id}/refund")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaymentDTO>> RefundPayment(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

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
        public async Task<ActionResult<bool>> VerifyPayment(int id)
        {
            try
            {
                var isVerified = await _paymentService.VerifyPaymentAsync(id);
                return Ok(isVerified);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying payment {PaymentId}", id);
                return StatusCode(500, "An error occurred while verifying the payment");
            }
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