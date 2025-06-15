using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;

namespace cookie_authentication_authorization_demo.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsBySellerIdAsync(int sellerId);
        Task<bool> UpdateProductStockAsync(int productId, int quantity);
        Task<bool> IsProductOwnerAsync(int productId, string userId);
        Task<ProductDTO> CreateProductFromViewModelAsync(CreateProductViewModel model, int sellerId);
        Task<ProductDTO> GetProductDTOByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllProductDTOsAsync();
        Task<IEnumerable<ProductDTO>> GetProductDTOsBySellerAsync(int sellerId);
        Task<IEnumerable<ProductDTO>> GetProductDTOsByCategoryAsync(string category);
        Task<ProductDTO> UpdateProductFromViewModelAsync(int id, UpdateProductViewModel model, int sellerId);
        Task<bool> DeleteProductWithAuditAsync(int id, int sellerId);
        Task<bool> UpdateStockWithAuditAsync(int id, int quantity, int sellerId);
    }
} 