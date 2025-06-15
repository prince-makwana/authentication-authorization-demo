using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<Product> GetByIdWithSellerAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetBySellerIdAsync(int sellerId);
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<Product> AddAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByNameAndSellerAsync(string name, int sellerId);
} 