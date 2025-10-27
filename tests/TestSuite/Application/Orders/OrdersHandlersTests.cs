using MediatR;
using Moq;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Orders;
using SolidApiExample.Domain.Shared;


namespace SolidApiExample.TestSuite.Application.Orders;

public sealed class OrdersHandlersTests
{
    private readonly Mock<IOrdersRepo> _repoMock = new();

    [Fact]
    public async Task GetOrder_Returns_Order_WhenFound()
    {
        var orderId = Guid.NewGuid();
        var expected = Order.FromExisting(
            orderId,
            Guid.NewGuid(),
            OrderStatus.Paid,
            Money.Create(25m, "USD"));

        _repoMock
            .Setup(m => m.FindAsync(orderId, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new GetOrderHandler(_repoMock.Object);

        var result = await handler.Handle(new GetOrderQuery(orderId), CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.CustomerId, result.CustomerId);
        Assert.Equal(ToDto(expected.Status), result.Status);
        Assert.Equal(expected.Total.Amount, result.Total.Amount);
        Assert.Equal(expected.Total.Currency, result.Total.Currency);
        _repoMock.Verify(m => m.FindAsync(orderId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrder_Throws_WhenNotFound()
    {
        var orderId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.FindAsync(orderId, CancellationToken.None))
            .ReturnsAsync((Order?)null);

        var handler = new GetOrderHandler(_repoMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new GetOrderQuery(orderId), CancellationToken.None));
        _repoMock.Verify(m => m.FindAsync(orderId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ListOrders_Returns_PagedOrders()
    {
        const int page = 1;
        const int size = 10;
        var expected = new Paged<Order>
        {
            Items = new List<Order>
            {
                Order.FromExisting(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    OrderStatus.New,
                    Money.Create(10m, "USD"))
            },
            Page = page,
            Size = size,
            Total = 5
        };

        _repoMock
            .Setup(m => m.ListAsync(page, size, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new ListOrdersHandler(_repoMock.Object);

        var result = await handler.Handle(new ListOrdersQuery(page, size), CancellationToken.None);

        Assert.Equal(expected.Total, result.Total);
        Assert.Equal(expected.Page, result.Page);
        Assert.Equal(expected.Size, result.Size);
        Assert.Single(result.Items);
        var expectedOrder = expected.Items[0];
        var resultOrder = result.Items[0];
        Assert.Equal(ToDto(expectedOrder.Status), resultOrder.Status);
        Assert.Equal(expectedOrder.Total.Amount, resultOrder.Total.Amount);
        Assert.Equal(expectedOrder.Total.Currency, resultOrder.Total.Currency);
        _repoMock.Verify(m => m.ListAsync(page, size, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateOrder_Forwards_ToRepository()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Total = new MoneyDto { Amount = 42m, Currency = "USD" }
        };
        _repoMock
            .Setup(m => m.AddAsync(It.IsAny<Order>(), CancellationToken.None))
            .ReturnsAsync((Order o, CancellationToken _) =>
                Order.FromExisting(o.Id, o.CustomerId, o.Status, o.Total));

        var handler = new CreateOrderHandler(_repoMock.Object);

        var result = await handler.Handle(new CreateOrderCommand(dto), CancellationToken.None);

        Assert.Equal(dto.CustomerId, result.CustomerId);
        Assert.Equal(OrderStatusDto.New, result.Status);
        Assert.Equal(dto.Total.Amount, result.Total.Amount);
        Assert.Equal(dto.Total.Currency, result.Total.Currency);
        _repoMock.Verify(
            m => m.AddAsync(
                It.Is<Order>(o =>
                    o.CustomerId == dto.CustomerId &&
                    o.Total.Amount == dto.Total.Amount &&
                    o.Total.Currency == dto.Total.Currency &&
                    o.Status == OrderStatus.New),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task UpdateOrder_Forwards_ToRepository()
    {
        var orderId = Guid.NewGuid();
        var dto = new UpdateOrderDto { Status = OrderStatusDto.Paid };
        var expected = Order.FromExisting(
            orderId,
            Guid.NewGuid(),
            OrderStatus.Paid,
            Money.Create(99m, "USD"));

        _repoMock
            .Setup(m => m.UpdateStatusAsync(orderId, OrderStatus.Paid, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new UpdateOrderHandler(_repoMock.Object);

        var result = await handler.Handle(new UpdateOrderCommand(orderId, dto), CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(OrderStatusDto.Paid, result.Status);
        _repoMock.Verify(m => m.UpdateStatusAsync(orderId, OrderStatus.Paid, CancellationToken.None), Times.Once);
    }

    private static OrderStatusDto ToDto(OrderStatus status) => status switch
    {
        OrderStatus.New => OrderStatusDto.New,
        OrderStatus.Paid => OrderStatusDto.Paid,
        OrderStatus.Shipped => OrderStatusDto.Shipped,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}
