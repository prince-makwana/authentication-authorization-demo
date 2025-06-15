using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Services;
using cookie_authentication_authorization_demo.Repositories;
using System.Security.Claims;
using System.Text.Json;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.Mappers;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Controllers;

/// <summary>
/// Controller for managing sellers in the e-commerce system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SellersController : ControllerBase
{
    private readonly ISellerRepository _sellerRepository;
    private readonly IAuditService _auditService;
    private readonly IProductService _productService;
    private readonly IInventoryService _inventoryService;

    /// <summary>
    /// Constructor for SellersController
    /// </summary>
    /// <param name="sellerRepository">Seller repository</param>
    /// <param name="auditService">Audit service</param>
    /// <param name="productService">Product service</param>
    /// <param name="inventoryService">Inventory service</param>
    /// <param name="inventoryMapper">Inventory mapper</param>
    public SellersController(ISellerRepository sellerRepository, IAuditService auditService, IProductService productService, IInventoryService inventoryService)
    {
        _sellerRepository = sellerRepository;
        _auditService = auditService;
        _productService = productService;
        _inventoryService = inventoryService;
    }

    /// <summary>
    /// Gets all sellers.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
    {
        var sellers = await _sellerRepository.GetAllAsync();
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
        var seller = await _sellerRepository.GetByIdAsync(id);
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
        seller.CreatedAt = DateTime.UtcNow;
        seller.IsActive = true;
        var createdSeller = await _sellerRepository.AddAsync(seller);
        await _auditService.LogActionAsync(
            "Seller",
            "Create",
            userId,
            JsonSerializer.Serialize(new
            {
                sellerId = createdSeller.Id,
                businessName = createdSeller.BusinessName,
                email = createdSeller.Email,
                taxId = createdSeller.TaxId
            })
        );
        return CreatedAtAction(nameof(GetSeller), new { id = createdSeller.Id }, createdSeller);
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
        var existingSeller = await _sellerRepository.GetByIdAsync(id);
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
        await _sellerRepository.UpdateAsync(existingSeller);
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

    /// <summary>
    /// Deletes a seller by ID.
    /// </summary>
    [Authorize(Policy = "RequireProductManagerRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeller(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var seller = await _sellerRepository.GetByIdAsync(id);
        if (seller == null || !seller.IsActive)
        {
            return NotFound("Seller not found");
        }
        // Soft delete
        seller.IsActive = false;
        seller.UpdatedAt = DateTime.UtcNow;
        await _sellerRepository.UpdateAsync(seller);
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

    [HttpGet("{id}/products")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetSellerProducts(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var products = await _productService.GetProductDTOsBySellerAsync(id);
        return Ok(products);
    }

    [HttpGet("{id}/inventory")]
    public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetSellerInventory(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var inventory = await _inventoryService.GetInventoryBySellerIdAsync(id);
        return Ok(inventory.Select(i => InventoryMapper.ToDTO(i)));
    }

    [HttpGet("{id}/inventory/status/{status}")]
    public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetSellerInventoryByStatus(int id, InventoryStatus status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var inventory = await _inventoryService.GetInventoryBySellerAndStatusAsync(id, status);
        return Ok(inventory.Select(i => InventoryMapper.ToDTO(i)));
    }

    [HttpGet("{id}/inventory/date-range")]
    public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetSellerInventoryByDateRange(
        int id,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var inventory = await _inventoryService.GetInventoryBySellerAndDateRangeAsync(id, startDate, endDate);
        return Ok(inventory.Select(i => InventoryMapper.ToDTO(i)));
    }

    [HttpGet("{id}/inventory/status/{status}/date-range")]
    public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetSellerInventoryByStatusAndDateRange(
        int id,
        InventoryStatus status,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var inventory = await _inventoryService.GetInventoryBySellerStatusAndDateRangeAsync(id, status, startDate, endDate);
        return Ok(inventory.Select(i => InventoryMapper.ToDTO(i)));
    }
} 