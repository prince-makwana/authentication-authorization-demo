using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Repositories;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger<InventoryService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public InventoryService(IInventoryRepository inventoryRepository, ILogger<InventoryService> logger, ApplicationDbContext context, IAuditService auditService)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
            _context = context;
            _auditService = auditService;
        }

        public async Task<bool> CheckStockAvailabilityAsync(int productId, int quantity)
        {
            var product = await _inventoryRepository.GetProductByIdAsync(productId);
            return product != null && product.StockQuantity >= quantity;
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

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerIdAsync(int sellerId)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId)
                .OrderByDescending(i => i.CreatedAt)
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

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerAndStatusAsync(int sellerId, InventoryStatus status)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId && i.Status == status)
                .OrderByDescending(i => i.CreatedAt)
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

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerAndDateRangeAsync(int sellerId, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .OrderByDescending(i => i.CreatedAt)
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

        public async Task<IEnumerable<Inventory>> GetInventoryBySellerStatusAndDateRangeAsync(int sellerId, InventoryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Include(i => i.Seller)
                .Where(i => i.SellerId == sellerId && i.Status == status && i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .OrderByDescending(i => i.CreatedAt)
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

        public async Task<bool> ReserveStockAsync(int productId, int quantity)
        {
            var product = await _inventoryRepository.GetProductByIdAsync(productId);
            if (product == null || product.StockQuantity < quantity)
                return false;
            product.StockQuantity -= quantity;
            await _inventoryRepository.UpdateProductStockAsync(productId, product.StockQuantity);
            return true;
        }

        public async Task<bool> ReleaseStockAsync(int productId, int quantity)
        {
            var product = await _inventoryRepository.GetProductByIdAsync(productId);
            if (product == null)
                return false;
            product.StockQuantity += quantity;
            await _inventoryRepository.UpdateProductStockAsync(productId, product.StockQuantity);
            return true;
        }
    }
} 