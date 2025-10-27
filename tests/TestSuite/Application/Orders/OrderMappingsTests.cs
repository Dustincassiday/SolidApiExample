using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.Orders;

namespace SolidApiExample.TestSuite.Application.Orders;

public sealed class OrderMappingsTests
{
    [Fact]
    public void ToDto_MapsOrderFields()
    {
        var order = Order.FromExisting(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Pending);

        var dto = order.ToDto();

        Assert.Equal(order.Id, dto.Id);
        Assert.Equal(order.PersonId, dto.PersonId);
        Assert.Equal(OrderStatusDto.Pending, dto.Status);
    }

    [Fact]
    public void ToDto_Paged_MapsCollection()
    {
        var order = Order.FromExisting(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Completed);
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
    }

    [Theory]
    [InlineData("Pending", OrderStatus.Pending)]
    [InlineData("completed", OrderStatus.Completed)]
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
        var status = OrderStatusDto.Cancelled.ToDomain();

        Assert.Equal(OrderStatus.Cancelled, status);
    }

    [Fact]
    public void ToDomain_Throws_WhenUnknown()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ((OrderStatusDto)999).ToDomain());
    }

    [Fact]
    public void ToDto_OrderStatus_ConvertsDomain()
    {
        var status = OrderStatus.Processing.ToDto();

        Assert.Equal(OrderStatusDto.Processing, status);
    }

    [Fact]
    public void ToDto_OrderStatus_Throws_WhenUnknown()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ((OrderStatus)999).ToDto());
    }
}
