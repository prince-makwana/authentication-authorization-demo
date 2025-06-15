using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Repositories;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(int id);
    Task<Order> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> AddAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByUserIdAndStatusAsync(string userId, OrderStatus status);
    Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
    Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
    Task<bool> DeleteOrderItemAsync(int id);
    Task<OrderItem> GetOrderItemByIdAsync(int id);
    Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
} 