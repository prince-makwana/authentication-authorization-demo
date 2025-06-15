using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;

namespace cookie_authentication_authorization_demo.Services
{
    /// <summary>
    /// Interface for inventory service operations
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        /// Checks if there is sufficient stock available for a product
        /// </summary>
        /// <param name="productId">ID of the product to check</param>
        /// <param name="quantity">Quantity to check availability for</param>
        /// <returns>True if sufficient stock is available, false otherwise</returns>
        Task<bool> CheckStockAvailabilityAsync(int productId, int quantity);

        /// <summary>
        /// Gets all inventory items
        /// </summary>
        /// <returns>List of all inventory items</returns>
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();

        /// <summary>
        /// Gets an inventory item by its ID
        /// </summary>
        /// <param name="id">ID of the inventory item</param>
        /// <returns>The inventory item if found, null otherwise</returns>
        Task<Inventory?> GetInventoryByIdAsync(int id);

        /// <summary>
        /// Gets all inventory items for a specific product
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <returns>List of inventory items for the product</returns>
        Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId);

        /// <summary>
        /// Creates a new inventory item
        /// </summary>
        /// <param name="inventory">Inventory information</param>
        /// <returns>The created inventory item</returns>
        Task<Inventory> CreateInventoryAsync(Inventory inventory);

        /// <summary>
        /// Updates an existing inventory item
        /// </summary>
        /// <param name="inventory">Updated inventory information</param>
        /// <returns>The updated inventory item</returns>
        Task<Inventory> UpdateInventoryAsync(Inventory inventory);

        /// <summary>
        /// Deletes an inventory item
        /// </summary>
        /// <param name="id">ID of the inventory item to delete</param>
        /// <returns>True if the inventory item was deleted, false otherwise</returns>
        Task<bool> DeleteInventoryAsync(int id);

        /// <summary>
        /// Gets all inventory items for a specific seller
        /// </summary>
        /// <param name="sellerId">ID of the seller</param>
        /// <returns>List of inventory items for the seller</returns>
        Task<IEnumerable<Inventory>> GetInventoryBySellerIdAsync(string sellerId);

        /// <summary>
        /// Gets all inventory items with a specific status
        /// </summary>
        /// <param name="status">Status of the inventory items</param>
        /// <returns>List of inventory items with the specified status</returns>
        Task<IEnumerable<Inventory>> GetInventoryByStatusAsync(InventoryStatus status);

        /// <summary>
        /// Gets all inventory items for a specific date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific quantity range
        /// </summary>
        /// <param name="minQuantity">Minimum quantity</param>
        /// <param name="maxQuantity">Maximum quantity</param>
        /// <returns>List of inventory items within the quantity range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeAsync(int minQuantity, int maxQuantity);

        /// <summary>
        /// Gets all inventory items for a specific price range
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <returns>List of inventory items within the price range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        /// <summary>
        /// Gets all inventory items for a specific location
        /// </summary>
        /// <param name="location">Location to search for</param>
        /// <returns>List of inventory items for the location</returns>
        Task<IEnumerable<Inventory>> GetInventoryByLocationAsync(string location);

        /// <summary>
        /// Gets all inventory items for a specific seller and status
        /// </summary>
        /// <param name="sellerId">ID of the seller</param>
        /// <param name="status">Status of the inventory items</param>
        /// <returns>List of inventory items for the seller with the specified status</returns>
        Task<IEnumerable<Inventory>> GetInventoryBySellerAndStatusAsync(string sellerId, InventoryStatus status);

        /// <summary>
        /// Gets all inventory items for a specific product and status
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="status">Status of the inventory items</param>
        /// <returns>List of inventory items for the product with the specified status</returns>
        Task<IEnumerable<Inventory>> GetInventoryByProductAndStatusAsync(int productId, InventoryStatus status);

        /// <summary>
        /// Gets all inventory items for a specific date range and status
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <param name="status">Status of the inventory items</param>
        /// <returns>List of inventory items within the date range with the specified status</returns>
        Task<IEnumerable<Inventory>> GetInventoryByDateRangeAndStatusAsync(DateTime startDate, DateTime endDate, InventoryStatus status);

        /// <summary>
        /// Gets all inventory items for a specific quantity range and status
        /// </summary>
        /// <param name="minQuantity">Minimum quantity</param>
        /// <param name="maxQuantity">Maximum quantity</param>
        /// <param name="status">Status of the inventory items</param>
        /// <returns>List of inventory items within the quantity range with the specified status</returns>
        Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeAndStatusAsync(int minQuantity, int maxQuantity, InventoryStatus status);

        /// <summary>
        /// Gets all inventory items for a specific price range and status
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <param name="status">Status of the inventory items</param>
        /// <returns>List of inventory items within the price range with the specified status</returns>
        Task<IEnumerable<Inventory>> GetInventoryByPriceRangeAndStatusAsync(decimal minPrice, decimal maxPrice, InventoryStatus status);

        /// <summary>
        /// Gets all inventory items for a specific location and status
        /// </summary>
        /// <param name="location">Location to search for</param>
        /// <param name="status">Status of the inventory items</param>
        /// <returns>List of inventory items for the location with the specified status</returns>
        Task<IEnumerable<Inventory>> GetInventoryByLocationAndStatusAsync(string location, InventoryStatus status);

        /// <summary>
        /// Gets all inventory items for a specific seller and date range
        /// </summary>
        /// <param name="sellerId">ID of the seller</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items for the seller within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryBySellerAndDateRangeAsync(string sellerId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific product and date range
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items for the product within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByProductAndDateRangeAsync(int productId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific quantity range and date range
        /// </summary>
        /// <param name="minQuantity">Minimum quantity</param>
        /// <param name="maxQuantity">Maximum quantity</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items within the quantity range and date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeAndDateRangeAsync(int minQuantity, int maxQuantity, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific price range and date range
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items within the price range and date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByPriceRangeAndDateRangeAsync(decimal minPrice, decimal maxPrice, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific location and date range
        /// </summary>
        /// <param name="location">Location to search for</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items for the location within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByLocationAndDateRangeAsync(string location, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific seller, status, and date range
        /// </summary>
        /// <param name="sellerId">ID of the seller</param>
        /// <param name="status">Status of the inventory items</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items for the seller with the specified status within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryBySellerStatusAndDateRangeAsync(string sellerId, InventoryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific product, status, and date range
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="status">Status of the inventory items</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items for the product with the specified status within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByProductStatusAndDateRangeAsync(int productId, InventoryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific quantity range, status, and date range
        /// </summary>
        /// <param name="minQuantity">Minimum quantity</param>
        /// <param name="maxQuantity">Maximum quantity</param>
        /// <param name="status">Status of the inventory items</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items within the quantity range with the specified status within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByQuantityRangeStatusAndDateRangeAsync(int minQuantity, int maxQuantity, InventoryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific price range, status, and date range
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <param name="status">Status of the inventory items</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items within the price range with the specified status within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByPriceRangeStatusAndDateRangeAsync(decimal minPrice, decimal maxPrice, InventoryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all inventory items for a specific location, status, and date range
        /// </summary>
        /// <param name="location">Location to search for</param>
        /// <param name="status">Status of the inventory items</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of inventory items for the location with the specified status within the date range</returns>
        Task<IEnumerable<Inventory>> GetInventoryByLocationStatusAndDateRangeAsync(string location, InventoryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Reserves a specified quantity of stock for a product.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="quantity">Quantity to reserve</param>
        /// <returns>True if stock was reserved successfully, false otherwise</returns>
        Task<bool> ReserveStockAsync(int productId, int quantity);

        /// <summary>
        /// Releases a specified quantity of stock for a product.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="quantity">Quantity to release</param>
        /// <returns>True if stock was released successfully, false otherwise</returns>
        Task<bool> ReleaseStockAsync(int productId, int quantity);
    }
} 