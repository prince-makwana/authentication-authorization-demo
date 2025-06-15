using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Services;
using System.Security.Claims;
using System.Text.Json;

namespace cookie_authentication_authorization_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public InventoryController(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        // GET: api/inventory
        /// <summary>
        /// Gets inventory details for all products, including seller information.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Administrator,InventoryManager")]
        public async Task<ActionResult<IEnumerable<object>>> GetInventory()
        {
            var inventory = await _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.StockQuantity,
                    p.SellerId,
                    SellerName = p.Seller != null ? p.Seller.BusinessName : null
                })
                .ToListAsync();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _auditService.LogActionAsync(
                    "Inventory",
                    "View",
                    userId,
                    JsonSerializer.Serialize(new { action = "ViewAll" })
                );
            }

            return Ok(inventory);
        }

        // POST: api/inventory
        [HttpPut("{id}/stock")]
        [Authorize(Roles = "Administrator,InventoryManager")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int newStockLevel)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var oldStock = product.StockQuantity;
            product.StockQuantity = newStockLevel;

            try
            {
                await _context.SaveChangesAsync();

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    await _auditService.LogActionAsync(
                        "Inventory",
                        "UpdateStock",
                        userId,
                        JsonSerializer.Serialize(new
                        {
                            productId = id,
                            oldStock,
                            newStock = newStockLevel,
                            productName = product.Name,
                            sellerName = product.Seller?.BusinessName
                        })
                    );
                }

                return Ok(new { message = "Stock updated successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/inventory/5
        [HttpDelete("{id}/stock")]
        public async Task<IActionResult> RemoveStock(int id, [FromBody] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var product = await _context.Products.FindAsync(id);
            if (product == null || !product.IsActive)
            {
                return NotFound("Product not found");
            }

            if (product.StockQuantity < quantity)
            {
                return BadRequest("Insufficient stock");
            }

            var oldQuantity = product.StockQuantity;
            product.StockQuantity -= quantity;
            product.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                await _auditService.LogActionAsync(
                    "Inventory",
                    "Update",
                    userId ?? "system",
                    JsonSerializer.Serialize(new
                    {
                        productId = id,
                        oldQuantity,
                        newQuantity = product.StockQuantity,
                        productName = product.Name,
                        action = "RemoveStock"
                    })
                );

                return Ok(new { 
                    ProductId = product.Id,
                    NewStockQuantity = product.StockQuantity,
                    UpdatedAt = product.UpdatedAt
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
} 