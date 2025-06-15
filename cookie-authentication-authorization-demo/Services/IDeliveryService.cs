using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;

namespace cookie_authentication_authorization_demo.Services
{
    /// <summary>
    /// Interface for delivery service operations
    /// </summary>
    public interface IDeliveryService
    {
        /// <summary>
        /// Gets all deliveries
        /// </summary>
        /// <returns>List of all deliveries</returns>
        Task<IEnumerable<Delivery>> GetAllDeliveriesAsync();

        /// <summary>
        /// Gets a delivery by its ID
        /// </summary>
        /// <param name="id">ID of the delivery</param>
        /// <returns>The delivery if found, null otherwise</returns>
        Task<Delivery?> GetDeliveryByIdAsync(int id);

        /// <summary>
        /// Gets all deliveries for a specific user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>List of deliveries for the user</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByUserIdAsync(string userId);

        /// <summary>
        /// Creates a new delivery
        /// </summary>
        /// <param name="delivery">Delivery information</param>
        /// <returns>The created delivery</returns>
        Task<Delivery> CreateDeliveryAsync(Delivery delivery);

        /// <summary>
        /// Updates an existing delivery
        /// </summary>
        /// <param name="delivery">Updated delivery information</param>
        /// <returns>The updated delivery</returns>
        Task<Delivery> UpdateDeliveryAsync(Delivery delivery);

        /// <summary>
        /// Deletes a delivery
        /// </summary>
        /// <param name="id">ID of the delivery to delete</param>
        /// <returns>True if the delivery was deleted, false otherwise</returns>
        Task<bool> DeleteDeliveryAsync(int id);

        /// <summary>
        /// Gets all deliveries for a specific order
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <returns>List of deliveries for the order</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByOrderIdAsync(int orderId);

        /// <summary>
        /// Gets all deliveries for a specific delivery person
        /// </summary>
        /// <param name="deliveryPersonId">ID of the delivery person</param>
        /// <returns>List of deliveries for the delivery person</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonIdAsync(string deliveryPersonId);

        /// <summary>
        /// Gets all deliveries with a specific status
        /// </summary>
        /// <param name="status">Status of the deliveries</param>
        /// <returns>List of deliveries with the specified status</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByStatusAsync(DeliveryStatus status);

        /// <summary>
        /// Gets all deliveries for a specific date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific address
        /// </summary>
        /// <param name="address">Address to search for</param>
        /// <returns>List of deliveries for the address</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByAddressAsync(string address);

        /// <summary>
        /// Gets all deliveries for a specific tracking number
        /// </summary>
        /// <param name="trackingNumber">Tracking number to search for</param>
        /// <returns>List of deliveries with the tracking number</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberAsync(string trackingNumber);

        /// <summary>
        /// Gets all deliveries for a specific delivery person and status
        /// </summary>
        /// <param name="deliveryPersonId">ID of the delivery person</param>
        /// <param name="status">Status of the deliveries</param>
        /// <returns>List of deliveries for the delivery person with the specified status</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonAndStatusAsync(string deliveryPersonId, DeliveryStatus status);

        /// <summary>
        /// Gets all deliveries for a specific user and status
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="status">Status of the deliveries</param>
        /// <returns>List of deliveries for the user with the specified status</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByUserAndStatusAsync(string userId, DeliveryStatus status);

        /// <summary>
        /// Gets all deliveries for a specific order and status
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="status">Status of the deliveries</param>
        /// <returns>List of deliveries for the order with the specified status</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByOrderAndStatusAsync(int orderId, DeliveryStatus status);

        /// <summary>
        /// Gets all deliveries for a specific date range and status
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <param name="status">Status of the deliveries</param>
        /// <returns>List of deliveries within the date range with the specified status</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByDateRangeAndStatusAsync(DateTime startDate, DateTime endDate, DeliveryStatus status);

        /// <summary>
        /// Gets all deliveries for a specific address and status
        /// </summary>
        /// <param name="address">Address to search for</param>
        /// <param name="status">Status of the deliveries</param>
        /// <returns>List of deliveries for the address with the specified status</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByAddressAndStatusAsync(string address, DeliveryStatus status);

        /// <summary>
        /// Gets all deliveries for a specific tracking number and status
        /// </summary>
        /// <param name="trackingNumber">Tracking number to search for</param>
        /// <param name="status">Status of the deliveries</param>
        /// <returns>List of deliveries with the tracking number and specified status</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberAndStatusAsync(string trackingNumber, DeliveryStatus status);

        /// <summary>
        /// Gets all deliveries for a specific delivery person and date range
        /// </summary>
        /// <param name="deliveryPersonId">ID of the delivery person</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the delivery person within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonAndDateRangeAsync(string deliveryPersonId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific user and date range
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the user within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByUserAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific order and date range
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the order within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByOrderAndDateRangeAsync(int orderId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific address and date range
        /// </summary>
        /// <param name="address">Address to search for</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the address within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByAddressAndDateRangeAsync(string address, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific tracking number and date range
        /// </summary>
        /// <param name="trackingNumber">Tracking number to search for</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries with the tracking number within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberAndDateRangeAsync(string trackingNumber, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific delivery person, status, and date range
        /// </summary>
        /// <param name="deliveryPersonId">ID of the delivery person</param>
        /// <param name="status">Status of the deliveries</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the delivery person with the specified status within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonStatusAndDateRangeAsync(string deliveryPersonId, DeliveryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific user, status, and date range
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="status">Status of the deliveries</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the user with the specified status within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByUserStatusAndDateRangeAsync(string userId, DeliveryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific order, status, and date range
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="status">Status of the deliveries</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the order with the specified status within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByOrderStatusAndDateRangeAsync(int orderId, DeliveryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific address, status, and date range
        /// </summary>
        /// <param name="address">Address to search for</param>
        /// <param name="status">Status of the deliveries</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries for the address with the specified status within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByAddressStatusAndDateRangeAsync(string address, DeliveryStatus status, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all deliveries for a specific tracking number, status, and date range
        /// </summary>
        /// <param name="trackingNumber">Tracking number to search for</param>
        /// <param name="status">Status of the deliveries</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of deliveries with the tracking number and specified status within the date range</returns>
        Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberStatusAndDateRangeAsync(string trackingNumber, DeliveryStatus status, DateTime startDate, DateTime endDate);
    }
} 