using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsBySellerIdAsync(string sellerId);
        Task<bool> UpdateProductStockAsync(int productId, int quantity);
    }
} 