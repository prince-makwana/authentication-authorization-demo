using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Services;
using System.Security.Claims;
using System.Text.Json;

namespace cookie_authentication_authorization_demo.Controllers;

/// <summary>
/// Controller for managing sellers in the e-commerce system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SellersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditService _auditService;

    /// <summary>
    /// Constructor for SellersController
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="auditService">Audit service</param>
    public SellersController(ApplicationDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    /// <summary>
    /// Gets all sellers.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
    {
        var sellers = await _context.Sellers.ToListAsync();

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await _auditService.LogActionAsync(
                "Seller",
                "View",
                userId,
                JsonSerializer.Serialize(new { action = "ViewAll" })
            );
        }

        return Ok(sellers);
    }

    /// <summary>
    /// Gets a seller by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Seller>> GetSeller(int id)
    {
        var seller = await _context.Sellers.FindAsync(id);

        if (seller == null)
        {
            return NotFound();
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await _auditService.LogActionAsync(
                "Seller",
                "View",
                userId,
                JsonSerializer.Serialize(new { sellerId = id, businessName = seller.BusinessName })
            );
        }

        return Ok(seller);
    }

    /// <summary>
    /// Creates a new seller.
    /// </summary>
    [Authorize(Policy = "RequireProductManagerRole")]
    [HttpPost]
    public async Task<ActionResult<Seller>> CreateSeller(Seller seller)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        seller.CreatedAt = DateTime.UtcNow;
        seller.IsActive = true;
        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();

        await _auditService.LogActionAsync(
            "Seller",
            "Create",
            userId,
            JsonSerializer.Serialize(new
            {
                sellerId = seller.Id,
                businessName = seller.BusinessName,
                email = seller.Email,
                taxId = seller.TaxId
            })
        );

        return CreatedAtAction(nameof(GetSeller), new { id = seller.Id }, seller);
    }

    /// <summary>
    /// Updates an existing seller.
    /// </summary>
    [Authorize(Policy = "RequireProductManagerRole")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSeller(int id, Seller seller)
    {
        if (id != seller.Id)
        {
            return BadRequest("Invalid seller ID");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        var existingSeller = await _context.Sellers.FindAsync(id);
        if (existingSeller == null || !existingSeller.IsActive)
        {
            return NotFound("Seller not found");
        }

        var oldValues = new
        {
            existingSeller.BusinessName,
            existingSeller.Email,
            existingSeller.PhoneNumber,
            existingSeller.Address
        };

        existingSeller.BusinessName = seller.BusinessName;
        existingSeller.Email = seller.Email;
        existingSeller.PhoneNumber = seller.PhoneNumber;
        existingSeller.Address = seller.Address;
        existingSeller.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "Seller",
                "Update",
                userId,
                JsonSerializer.Serialize(new
                {
                    sellerId = id,
                    oldSeller = oldValues,
                    newSeller = new
                    {
                        existingSeller.BusinessName,
                        existingSeller.Email,
                        existingSeller.PhoneNumber,
                        existingSeller.Address
                    }
                })
            );

            return Ok(new { 
                Id = existingSeller.Id,
                BusinessName = existingSeller.BusinessName,
                UpdatedAt = existingSeller.UpdatedAt
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SellerExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Deletes a seller by ID.
    /// </summary>
    [Authorize(Policy = "RequireProductManagerRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeller(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        var seller = await _context.Sellers.FindAsync(id);
        if (seller == null || !seller.IsActive)
        {
            return NotFound("Seller not found");
        }

        // Soft delete
        seller.IsActive = false;
        seller.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        await _auditService.LogActionAsync(
            "Seller",
            "Delete",
            userId,
            JsonSerializer.Serialize(new
            {
                sellerId = id,
                businessName = seller.BusinessName,
                email = seller.Email
            })
        );

        return Ok(new { 
            Id = seller.Id,
            BusinessName = seller.BusinessName,
            DeletedAt = seller.UpdatedAt
        });
    }

    private bool SellerExists(int id)
    {
        return _context.Sellers.Any(e => e.Id == id);
    }
} 