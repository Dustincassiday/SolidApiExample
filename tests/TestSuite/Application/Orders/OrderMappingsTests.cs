using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.Orders;
using SolidApiExample.Domain.Shared;

namespace SolidApiExample.TestSuite.Application.Orders;

public sealed class OrderMappingsTests
{
    [Fact]
    public void ToDto_MapsOrderFields()
    {
        var order = Order.FromExisting(
            Guid.NewGuid(),
            Guid.NewGuid(),
            OrderStatus.Paid,
            Money.Create(12.34m, "USD"));

        var dto = order.ToDto();

        Assert.Equal(order.Id, dto.Id);
        Assert.Equal(order.CustomerId, dto.CustomerId);
        Assert.Equal(OrderStatusDto.Paid, dto.Status);
        Assert.Equal(order.Total.Amount, dto.Total.Amount);
        Assert.Equal(order.Total.Currency, dto.Total.Currency);
    }

    [Fact]
    public void ToDto_Paged_MapsCollection()
    {
        var order = Order.FromExisting(
            Guid.NewGuid(),
            Guid.NewGuid(),
            OrderStatus.Shipped,
            Money.Create(50m, "USD"));
        var paged = new Paged<Order>
        {
            Items = new List<Order> { order },
            Page = 2,
            Size = 5,
            Total = 12
        };

        var result = paged.ToDto();

        Assert.Single(result.Items);
        Assert.Equal(order.Id, result.Items[0].Id);
        Assert.Equal(paged.Page, result.Page);
        Assert.Equal(paged.Size, result.Size);
        Assert.Equal(paged.Total, result.Total);
        Assert.Equal(order.Total.Amount, result.Items[0].Total.Amount);
        Assert.Equal(order.Total.Currency, result.Items[0].Total.Currency);
    }

    [Theory]
    [InlineData("New", OrderStatus.New)]
    [InlineData("paid", OrderStatus.Paid)]
    public void ToOrderStatus_ParsesValues(string input, OrderStatus expected)
    {
        var status = input.ToOrderStatus();

        Assert.Equal(expected, status);
    }

    [Fact]
    public void ToOrderStatus_ThrowsValidationException_WhenInvalid()
    {
        var ex = Assert.Throws<ValidationException>(() => "unknown".ToOrderStatus());
        Assert.Contains("Order status 'unknown' is not valid.", ex.Errors);
    }

    [Fact]
    public void ToDomain_ConvertsDto()
    {
        var status = OrderStatusDto.Shipped.ToDomain();

        Assert.Equal(OrderStatus.Shipped, status);
    }

    [Fact]
    public void ToDomain_Throws_WhenUnknown()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ((OrderStatusDto)999).ToDomain());
    }

    [Fact]
    public void ToDto_OrderStatus_ConvertsDomain()
    {
        var status = OrderStatus.Paid.ToDto();

        Assert.Equal(OrderStatusDto.Paid, status);
    }

    [Fact]
    public void ToDto_OrderStatus_Throws_WhenUnknown()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ((OrderStatus)999).ToDto());
    }
}
