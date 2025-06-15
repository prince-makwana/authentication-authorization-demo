using System;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;

namespace cookie_authentication_authorization_demo.Mappers;

public static class OrderItemMapper
{
    public static OrderItemDTO ToDTO(OrderItem orderItem)
    {
        if (orderItem == null)
            return null;

        return new OrderItemDTO
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice
        };
    }

    public static OrderItem ToEntity(OrderItemDTO dto)
    {
        if (dto == null)
            return null;

        return new OrderItem
        {
            Id = dto.Id,
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };
    }

    public static OrderItem ToEntity(CreateOrderItemViewModel viewModel)
    {
        if (viewModel == null)
            return null;

        return new OrderItem
        {
            ProductId = viewModel.ProductId,
            Quantity = viewModel.Quantity
        };
    }
} 