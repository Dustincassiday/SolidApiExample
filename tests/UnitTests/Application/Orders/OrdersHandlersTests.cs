using Moq;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Orders;


namespace SolidApiExample.UnitTests.Application.Orders;

public sealed class OrdersHandlersTests
{
    private readonly Mock<IOrdersRepo> _repoMock = new();

    [Fact]
    public async Task GetOrder_Returns_Order_WhenFound()
    {
        var orderId = Guid.NewGuid();
        var expected = Order.FromExisting(orderId, Guid.NewGuid(), OrderStatus.Pending);

        _repoMock
            .Setup(m => m.FindAsync(orderId, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new GetOrder(_repoMock.Object);

        var result = await handler.GetAsync(orderId, CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.PersonId, result.PersonId);
        Assert.Equal(ToDto(expected.Status), result.Status);
        _repoMock.Verify(m => m.FindAsync(orderId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrder_Throws_WhenNotFound()
    {
        var orderId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.FindAsync(orderId, CancellationToken.None))
            .ReturnsAsync((Order?)null);

        var handler = new GetOrder(_repoMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.GetAsync(orderId, CancellationToken.None));
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
                Order.FromExisting(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Pending)
            },
            Page = page,
            Size = size,
            Total = 5
        };

        _repoMock
            .Setup(m => m.ListAsync(page, size, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new ListOrders(_repoMock.Object);

        var result = await handler.ListAsync(page, size, CancellationToken.None);

        Assert.Equal(expected.Total, result.Total);
        Assert.Equal(expected.Page, result.Page);
        Assert.Equal(expected.Size, result.Size);
        Assert.Single(result.Items);
        Assert.Equal(ToDto(expected.Items.First().Status), result.Items.First().Status);
        _repoMock.Verify(m => m.ListAsync(page, size, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateOrder_Forwards_ToRepository()
    {
        var dto = new CreateOrderDto { PersonId = Guid.NewGuid(), Status = OrderStatusDto.Pending };
        _repoMock
            .Setup(m => m.AddAsync(It.IsAny<Order>(), CancellationToken.None))
            .ReturnsAsync((Order o, CancellationToken _) => Order.FromExisting(o.Id, o.PersonId, o.Status));

        var handler = new CreateOrder(_repoMock.Object);

        var result = await handler.CreateAsync(dto, CancellationToken.None);

        Assert.Equal(dto.PersonId, result.PersonId);
        Assert.Equal(dto.Status, result.Status);
        _repoMock.Verify(m => m.AddAsync(It.Is<Order>(o => o.PersonId == dto.PersonId && o.Status == OrderStatus.Pending), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateOrder_Forwards_ToRepository()
    {
        var orderId = Guid.NewGuid();
        var dto = new UpdateOrderDto { Status = OrderStatusDto.Completed };
        var expected = Order.FromExisting(orderId, Guid.NewGuid(), OrderStatus.Completed);

        _repoMock
            .Setup(m => m.UpdateStatusAsync(orderId, OrderStatus.Completed, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new UpdateOrder(_repoMock.Object);

        var result = await handler.UpdateAsync(orderId, dto, CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(OrderStatusDto.Completed, result.Status);
        _repoMock.Verify(m => m.UpdateStatusAsync(orderId, OrderStatus.Completed, CancellationToken.None), Times.Once);
    }

    private static OrderStatusDto ToDto(OrderStatus status) => status switch
    {
        OrderStatus.Pending => OrderStatusDto.Pending,
        OrderStatus.Processing => OrderStatusDto.Processing,
        OrderStatus.Completed => OrderStatusDto.Completed,
        OrderStatus.Cancelled => OrderStatusDto.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}
