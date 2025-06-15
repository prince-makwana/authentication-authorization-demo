using System;
using System.Linq;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;

namespace cookie_authentication_authorization_demo.Mappers;

public static class OrderMapper
{
    public static OrderDTO ToDTO(Order order)
    {
        if (order == null)
            return null;

        return new OrderDTO
        {
            Id = order.Id,
            UserId = order.UserId,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt ?? DateTime.UtcNow,
            OrderItems = order.OrderItems?.Select(item => OrderItemMapper.ToDTO(item)).ToList()
        };
    }

    public static Order ToEntity(OrderDTO dto)
    {
        if (dto == null)
            return null;

        return new Order
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Status = dto.Status,
            TotalAmount = dto.TotalAmount,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            OrderItems = dto.OrderItems?.Select(item => OrderItemMapper.ToEntity(item)).ToList()
        };
    }
} 