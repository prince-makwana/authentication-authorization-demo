using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task<bool> CancelOrderAsync(int id);
        Task<bool> ProcessOrderAsync(int id);
        Task<bool> CompleteOrderAsync(int id);
    }
} 