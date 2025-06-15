using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;
        private readonly IInventoryService _inventoryService;

        public OrderService(
            ApplicationDbContext context,
            IAuditService auditService,
            IInventoryService inventoryService)
        {
            _context = context;
            _auditService = auditService;
            _inventoryService = inventoryService;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Validate stock availability
            foreach (var item in order.OrderItems)
            {
                var isAvailable = await _inventoryService.CheckStockAvailabilityAsync(
                    item.ProductId,
                    item.Quantity);
                if (!isAvailable)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for product ID {item.ProductId}");
                }
            }

            // Reserve stock
            foreach (var item in order.OrderItems)
            {
                await _inventoryService.ReserveStockAsync(
                    item.ProductId,
                    item.Quantity);
            }

            order.CreatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return false;
            }

            if (order.Status == OrderStatus.Delivered)
            {
                throw new InvalidOperationException("Cannot cancel a delivered order");
            }

            // Release reserved stock
            foreach (var item in order.OrderItems)
            {
                await _inventoryService.ReleaseStockAsync(
                    item.ProductId,
                    item.Quantity);
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProcessOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Order is not in pending status");
            }

            order.Status = OrderStatus.Processing;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            if (order.Status != OrderStatus.Processing)
            {
                throw new InvalidOperationException("Order is not in processing status");
            }

            order.Status = OrderStatus.Delivered;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 