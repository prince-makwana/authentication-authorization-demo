using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public interface IInventoryRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    Task<Product> UpdateProductStockAsync(int productId, int newQuantity);
    Task<bool> ExistsAsync(int productId);
} 