using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;
using cookie_authentication_authorization_demo.Repositories;
using cookie_authentication_authorization_demo.Mappers;
using cookie_authentication_authorization_demo.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace cookie_authentication_authorization_demo.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(OrderMapper.ToDTO);
    }

    public async Task<OrderDTO> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order != null ? OrderMapper.ToDTO(order) : null!;
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(string userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);
        return orders.Select(OrderMapper.ToDTO);
    }

    public async Task<OrderDTO> CreateOrderAsync(CreateOrderViewModel model, string userId)
    {
        try
        {
            // Validate order items
            foreach (var item in model.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {item.ProductId} not found");
                }

                var inventory = await _inventoryRepository.GetProductByIdAsync(item.ProductId);
                if (inventory == null || inventory.StockQuantity < item.Quantity)
                {
                    throw new ArgumentException($"Insufficient inventory for product {product.Name}");
                }
            }

            // Create order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = userId,
                ShippingAddress = model.ShippingAddress,
                Status = Enums.OrderStatus.Pending,
                OrderItems = model.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            // Calculate total amount
            order.TotalAmount = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);

            // Save order
            await _orderRepository.AddAsync(order);

            // Update inventory
            foreach (var item in order.OrderItems)
            {
                var inventory = await _inventoryRepository.GetProductByIdAsync(item.ProductId);
                if (inventory != null)
                {
                    await _inventoryRepository.UpdateProductStockAsync(item.ProductId, inventory.StockQuantity - item.Quantity);
                }
            }

            return OrderMapper.ToDTO(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order for user {UserId}", userId);
            throw;
        }
    }

    public async Task<OrderDTO> UpdateOrderStatusAsync(int id, OrderStatus status, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            throw new ArgumentException($"Order with ID {id} not found");
        }

        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this order");
        }

        if (!IsValidStatusTransition(order.Status, status))
        {
            throw new InvalidOperationException($"Cannot transition from {order.Status} to {status}");
        }

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);
        return OrderMapper.ToDTO(order);
    }

    public async Task<bool> CancelOrderAsync(int id, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            throw new ArgumentException($"Order with ID {id} not found");
        }

        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to cancel this order");
        }

        if (order.Status != Enums.OrderStatus.Pending && order.Status != Enums.OrderStatus.Confirmed)
        {
            throw new InvalidOperationException("Only pending or confirmed orders can be cancelled");
        }

        order.Status = Enums.OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        // Restore inventory
        foreach (var item in order.OrderItems)
        {
            var inventory = await _inventoryRepository.GetProductByIdAsync(item.ProductId);
            if (inventory != null)
            {
                await _inventoryRepository.UpdateProductStockAsync(item.ProductId, inventory.StockQuantity + item.Quantity);
            }
        }

        await _orderRepository.UpdateAsync(order);
        return true;
    }

    public async Task<bool> ProcessOrderAsync(int id, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            throw new ArgumentException($"Order with ID {id} not found");
        }

        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to process this order");
        }

        if (order.Status != Enums.OrderStatus.Confirmed)
        {
            throw new InvalidOperationException("Only confirmed orders can be processed");
        }

        order.Status = Enums.OrderStatus.Processing;
        order.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);
        return true;
    }

    public async Task<bool> CompleteOrderAsync(int id, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            throw new ArgumentException($"Order with ID {id} not found");
        }

        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to complete this order");
        }

        if (order.Status != Enums.OrderStatus.Processing)
        {
            throw new InvalidOperationException("Only processing orders can be completed");
        }

        order.Status = Enums.OrderStatus.Delivered;
        order.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);
        return true;
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
    }

    private bool IsValidStatusTransition(Enums.OrderStatus currentStatus, Enums.OrderStatus newStatus)
    {
        return (currentStatus, newStatus) switch
        {
            (Enums.OrderStatus.Pending, Enums.OrderStatus.Confirmed) => true,
            (Enums.OrderStatus.Confirmed, Enums.OrderStatus.Processing) => true,
            (Enums.OrderStatus.Processing, Enums.OrderStatus.Shipped) => true,
            (Enums.OrderStatus.Shipped, Enums.OrderStatus.Delivered) => true,
            (Enums.OrderStatus.Pending, Enums.OrderStatus.Cancelled) => true,
            (Enums.OrderStatus.Confirmed, Enums.OrderStatus.Cancelled) => true,
            (Enums.OrderStatus.Delivered, Enums.OrderStatus.Refunded) => true,
            _ => false
        };
    }
} 