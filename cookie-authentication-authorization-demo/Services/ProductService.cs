using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;
using cookie_authentication_authorization_demo.Repositories;
using cookie_authentication_authorization_demo.Mappers;
using cookie_authentication_authorization_demo.Data;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Enums;
using Microsoft.AspNetCore.Identity;

namespace cookie_authentication_authorization_demo.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IAuditService _auditService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ProductService(
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository,
        ILogger<ProductService> logger,
        IAuditService auditService,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
        _logger = logger;
        _auditService = auditService;
        _userManager = userManager;
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdWithSellerAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProductsBySellerIdAsync(int sellerId)
    {
        return await _productRepository.GetBySellerIdAsync(sellerId);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (await _productRepository.ExistsByNameAndSellerAsync(product.Name, product.SellerId))
        {
            throw new InvalidOperationException("A product with this name already exists for this seller.");
        }

        return await _productRepository.AddAsync(product);
    }

    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (existingProduct.SellerId != product.SellerId)
        {
            throw new InvalidOperationException("Cannot change the seller of a product.");
        }

        if (product.Name != existingProduct.Name &&
            await _productRepository.ExistsByNameAndSellerAsync(product.Name, product.SellerId))
        {
            throw new InvalidOperationException("A product with this name already exists for this seller.");
        }

        product.Id = id;
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    public async Task<bool> UpdateProductStockAsync(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            return false;
        }

        product.StockQuantity = quantity;
        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> IsProductOwnerAsync(int productId, string userId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        if (!int.TryParse(userId, out int userIdInt))
        {
            return false;
        }

        return product.SellerId == userIdInt;
    }

    public async Task<ProductDTO> CreateProductFromViewModelAsync(CreateProductViewModel model, int sellerId)
    {
        try
        {
            if (await _productRepository.ExistsByNameAndSellerAsync(model.Name, sellerId))
            {
                throw new InvalidOperationException($"A product with name '{model.Name}' already exists for this seller");
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                Category = model.Category,
                ImageUrl = model.ImageUrl,
                SellerId = sellerId,
                Status = ProductStatus.InStock,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            product = await _productRepository.AddAsync(product);

            // Update inventory using the correct method
            await _inventoryRepository.UpdateProductStockAsync(product.Id, model.StockQuantity);

            await _auditService.LogActionAsync(
                "Product",
                "Create",
                sellerId.ToString(),
                System.Text.Json.JsonSerializer.Serialize(new { productId = product.Id, name = product.Name })
            );

            return ProductMapper.ToDTO(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product for seller {SellerId}", sellerId);
            throw;
        }
    }

    public async Task<ProductDTO> GetProductDTOByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdWithSellerAsync(id);
        return product != null ? ProductMapper.ToDTO(product) : null;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductDTOsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => ProductMapper.ToDTO(p));
    }

    public async Task<IEnumerable<ProductDTO>> GetProductDTOsBySellerAsync(int sellerId)
    {
        var products = await _productRepository.GetBySellerIdAsync(sellerId);
        return products.Select(p => ProductMapper.ToDTO(p));
    }

    public async Task<IEnumerable<ProductDTO>> GetProductDTOsByCategoryAsync(string category)
    {
        var products = await _productRepository.GetByCategoryAsync(category);
        return products.Select(p => ProductMapper.ToDTO(p));
    }

    public async Task<ProductDTO> UpdateProductFromViewModelAsync(int id, UpdateProductViewModel model, int sellerId)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (existingProduct.SellerId != sellerId)
        {
            throw new InvalidOperationException("Cannot update a product that doesn't belong to you.");
        }

        if (model.Name != existingProduct.Name &&
            await _productRepository.ExistsByNameAndSellerAsync(model.Name, sellerId))
        {
            throw new InvalidOperationException($"A product with name '{model.Name}' already exists for this seller");
        }

        existingProduct.Name = model.Name;
        existingProduct.Description = model.Description;
        existingProduct.Price = model.Price;
        existingProduct.StockQuantity = model.StockQuantity;
        existingProduct.Category = model.Category;
        existingProduct.ImageUrl = model.ImageUrl;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(existingProduct);
        await _inventoryRepository.UpdateProductStockAsync(id, model.StockQuantity);

        await _auditService.LogActionAsync(
            "Product",
            "Update",
            sellerId.ToString(),
            System.Text.Json.JsonSerializer.Serialize(new { productId = id, name = model.Name })
        );

        return ProductMapper.ToDTO(existingProduct);
    }

    public async Task<bool> DeleteProductWithAuditAsync(int id, int sellerId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        if (product.SellerId != sellerId)
        {
            throw new InvalidOperationException("Cannot delete a product that doesn't belong to you.");
        }

        var result = await _productRepository.DeleteAsync(id);
        if (result)
        {
            await _auditService.LogActionAsync(
                "Product",
                "Delete",
                sellerId.ToString(),
                System.Text.Json.JsonSerializer.Serialize(new { productId = id, name = product.Name })
            );
        }

        return result;
    }

    public async Task<bool> UpdateStockWithAuditAsync(int id, int quantity, int sellerId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        if (product.SellerId != sellerId)
        {
            throw new InvalidOperationException("Cannot update stock for a product that doesn't belong to you.");
        }

        product.StockQuantity = quantity;
        await _productRepository.UpdateAsync(product);

        await _auditService.LogActionAsync(
            "Product",
            "UpdateStock",
            sellerId.ToString(),
            System.Text.Json.JsonSerializer.Serialize(new { productId = id, newQuantity = quantity })
        );

        return true;
    }
} 