using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;

namespace cookie_authentication_authorization_demo.Models.Mappers
{
    public static class InventoryMapper
    {
        public static InventoryDTO ToDTO(Inventory inventory)
        {
            if (inventory == null)
                return null;

            return new InventoryDTO
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product?.Name ?? string.Empty,
                SellerId = inventory.SellerId,
                SellerName = inventory.Seller?.BusinessName ?? string.Empty,
                Quantity = inventory.Quantity,
                Price = inventory.Price,
                Location = inventory.Location,
                Status = inventory.Status,
                CreatedAt = inventory.CreatedAt,
                UpdatedAt = inventory.UpdatedAt
            };
        }

        public static Inventory ToEntity(InventoryDTO dto)
        {
            if (dto == null)
                return null;

            return new Inventory
            {
                Id = dto.Id,
                ProductId = dto.ProductId,
                SellerId = dto.SellerId,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Location = dto.Location,
                Status = dto.Status,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }
    }
} 