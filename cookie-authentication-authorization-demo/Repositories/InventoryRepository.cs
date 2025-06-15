using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly ApplicationDbContext _context;

    public InventoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
    {
        return await _context.Products.Where(p => p.StockQuantity <= threshold).ToListAsync();
    }

    public async Task<Product> UpdateProductStockAsync(int productId, int newQuantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return null;
        product.StockQuantity = newQuantity;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> ExistsAsync(int productId)
    {
        return await _context.Products.AnyAsync(p => p.Id == productId);
    }
} 