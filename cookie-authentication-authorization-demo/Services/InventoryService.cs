using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace cookie_authentication_authorization_demo.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public InventoryService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .ToListAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(int id)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
        {
            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<Inventory> UpdateInventoryAsync(Inventory inventory)
        {
            _context.Inventory.Update(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<bool> DeleteInventoryAsync(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
                return false;

            _context.Inventory.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerIdAsync(string sellerId)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByStatusAsync(InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeAsync(int minQuantity, int maxQuantity)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Quantity >= minQuantity && i.Quantity <= maxQuantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Price >= minPrice && i.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByLocationAsync(string location)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Location.Contains(location))
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerAndStatusAsync(string sellerId, InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId && i.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByProductAndStatusAsync(int productId, InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.ProductId == productId && i.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByDateRangeAndStatusAsync(DateTime startDate, DateTime endDate, InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate && i.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeAndStatusAsync(int minQuantity, int maxQuantity, InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Quantity >= minQuantity && i.Quantity <= maxQuantity && i.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByPriceRangeAndStatusAsync(decimal minPrice, decimal maxPrice, InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByLocationAndStatusAsync(string location, InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Location.Contains(location) && i.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerAndDateRangeAsync(string sellerId, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByProductAndDateRangeAsync(int productId, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.ProductId == productId && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeAndDateRangeAsync(int minQuantity, int maxQuantity, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Quantity >= minQuantity && i.Quantity <= maxQuantity && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByPriceRangeAndDateRangeAsync(decimal minPrice, decimal maxPrice, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByLocationAndDateRangeAsync(string location, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Location.Contains(location) && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerStatusAndDateRangeAsync(string sellerId, InventoryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId && i.Status == status && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByProductStatusAndDateRangeAsync(int productId, InventoryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.ProductId == productId && i.Status == status && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeStatusAndDateRangeAsync(int minQuantity, int maxQuantity, InventoryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Quantity >= minQuantity && i.Quantity <= maxQuantity && i.Status == status && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByPriceRangeStatusAndDateRangeAsync(decimal minPrice, decimal maxPrice, InventoryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.Status == status && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByLocationStatusAndDateRangeAsync(string location, InventoryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.Location.Contains(location) && i.Status == status && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if there is sufficient stock available for a product
        /// </summary>
        /// <param name="productId">ID of the product to check</param>
        /// <param name="quantity">Quantity to check availability for</param>
        /// <returns>True if sufficient stock is available, false otherwise</returns>
        public async Task<bool> CheckStockAvailabilityAsync(int productId, int quantity)
        {
            var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ProductId == productId);
            return inventoryItem != null && inventoryItem.Quantity >= quantity;
        }

        /// <summary>
        /// Reserves a specified quantity of stock for a product.
        /// Decreases the stock quantity and logs the action.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="quantity">Quantity to reserve</param>
        /// <returns>True if stock was reserved successfully, false otherwise</returns>
        public async Task<bool> ReserveStockAsync(int productId, int quantity)
        {
            var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ProductId == productId);
            if (inventoryItem == null || inventoryItem.Quantity < quantity)
            {
                return false; // Not enough stock or product not found in inventory
            }

            inventoryItem.Quantity -= quantity;
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "Inventory",
                "ReserveStock",
                null,
                JsonSerializer.Serialize(new
                {
                    productId,
                    oldQuantity = inventoryItem.Quantity + quantity,
                    newQuantity = inventoryItem.Quantity,
                    action = "ReserveStock"
                })
            );
            return true;
        }

        /// <summary>
        /// Releases a specified quantity of stock for a product.
        /// Increases the stock quantity and logs the action.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="quantity">Quantity to release</param>
        /// <returns>True if stock was released successfully, false otherwise</returns>
        public async Task<bool> ReleaseStockAsync(int productId, int quantity)
        {
            var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ProductId == productId);
            if (inventoryItem == null)
            {
                return false; // Product not found in inventory
            }

            inventoryItem.Quantity += quantity;
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "Inventory",
                "ReleaseStock",
                null,
                JsonSerializer.Serialize(new
                {
                    productId,
                    oldQuantity = inventoryItem.Quantity - quantity,
                    newQuantity = inventoryItem.Quantity,
                    action = "ReleaseStock"
                })
            );
            return true;
        }

        public async Task<bool> AddStockAsync(int productId, int quantity)
        {
            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(i => i.ProductId == productId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Inventory.Add(inventory);
            }
            else
            {
                inventory.Quantity += quantity;
                inventory.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStockAsync(int productId, int quantity)
        {
            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(i => i.ProductId == productId);

            if (inventory == null || inventory.Quantity < quantity)
            {
                return false;
            }

            inventory.Quantity -= quantity;
            inventory.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            try
            {
                await _auditService.LogActionAsync(
                    "Inventory",
                    "RemoveStock",
                    null,
                    JsonSerializer.Serialize(new
                    {
                        productId,
                        oldQuantity = inventory.Quantity + quantity,
                        newQuantity = inventory.Quantity,
                        action = "RemoveStock"
                    })
                );

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                Console.WriteLine($"Error logging action: {ex.Message}");
                return false;
            }
        }
    }
} 