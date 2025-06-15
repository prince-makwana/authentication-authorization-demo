using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(string userId);
        Task<OrderDTO> CreateOrderAsync(CreateOrderViewModel model, string userId);
        Task<OrderDTO> UpdateOrderStatusAsync(int id, Enums.OrderStatus status, string userId);
        Task<bool> CancelOrderAsync(int id, string userId);
        Task<bool> ProcessOrderAsync(int id, string userId);
        Task<bool> CompleteOrderAsync(int id, string userId);
    }
} 