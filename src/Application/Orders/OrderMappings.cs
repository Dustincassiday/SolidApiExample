using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.Orders;

namespace SolidApiExample.Application.Orders;

internal static class OrderMappings
{
    public static OrderDto ToDto(this Order order) =>
        new()
        {
            Id = order.Id,
            PersonId = order.PersonId,
            Status = order.Status.ToDto()
        };

    public static Paged<OrderDto> ToDto(this Paged<Order> orders) =>
        new()
        {
            Items = orders.Items.Select(o => o.ToDto()).ToList(),
            Page = orders.Page,
            Size = orders.Size,
            Total = orders.Total
        };

    public static OrderStatus ToOrderStatus(this string value)
    {
        if (!Enum.TryParse<OrderStatus>(value, true, out var status))
        {
            throw new ValidationException(new[] { $"Order status '{value}' is not valid." });
        }

        return status;
    }

    public static OrderStatus ToDomain(this OrderStatusDto status) => status switch
    {
        OrderStatusDto.Pending => OrderStatus.Pending,
        OrderStatusDto.Processing => OrderStatus.Processing,
        OrderStatusDto.Completed => OrderStatus.Completed,
        OrderStatusDto.Cancelled => OrderStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    public static OrderStatusDto ToDto(this OrderStatus status) => status switch
    {
        OrderStatus.Pending => OrderStatusDto.Pending,
        OrderStatus.Processing => OrderStatusDto.Processing,
        OrderStatus.Completed => OrderStatusDto.Completed,
        OrderStatus.Cancelled => OrderStatusDto.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}
