using System.Linq;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Mappers;

public static class ProductMapper
{
    public static ProductDTO ToDTO(Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            SellerId = product.SellerId,
            SellerName = product.Seller?.BusinessName ?? "Unknown Seller",
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            Status = product.Status,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt ?? DateTime.UtcNow
        };
    }

    public static Product ToEntity(ProductDTO productDTO)
    {
        return new Product
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            Category = productDTO.Category,
            SellerId = productDTO.SellerId,
            StockQuantity = productDTO.StockQuantity,
            ImageUrl = productDTO.ImageUrl,
            Status = productDTO.Status,
            IsActive = productDTO.IsActive,
            CreatedAt = productDTO.CreatedAt,
            UpdatedAt = productDTO.UpdatedAt
        };
    }
} 