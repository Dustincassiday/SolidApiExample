using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using Xunit;

namespace SolidApiExample.UnitTests.Application.Orders;

public sealed class OrdersHandlersTests
{
    private readonly Mock<IOrdersRepo> _repoMock = new();

    [Fact]
    public async Task GetOrder_Returns_Order_WhenFound()
    {
        var orderId = Guid.NewGuid();
        var expected = new OrderDto { Id = orderId, PersonId = Guid.NewGuid(), Status = "Pending" };

        _repoMock
            .Setup(m => m.FindAsync(orderId, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new GetOrder(_repoMock.Object);

        var result = await handler.GetAsync(orderId, CancellationToken.None);

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.FindAsync(orderId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrder_Throws_WhenNotFound()
    {
        var orderId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.FindAsync(orderId, CancellationToken.None))
            .ReturnsAsync((OrderDto?)null);

        var handler = new GetOrder(_repoMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.GetAsync(orderId, CancellationToken.None));
        _repoMock.Verify(m => m.FindAsync(orderId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ListOrders_Returns_PagedOrders()
    {
        const int page = 1;
        const int size = 10;
        var expected = new Paged<OrderDto>
        {
            Items = new List<OrderDto>
            {
                new() { Id = Guid.NewGuid(), PersonId = Guid.NewGuid(), Status = "Pending" }
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

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.ListAsync(page, size, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateOrder_Forwards_ToRepository()
    {
        var dto = new CreateOrderDto { PersonId = Guid.NewGuid(), Status = "Pending" };
        var expected = new OrderDto { Id = Guid.NewGuid(), PersonId = dto.PersonId, Status = dto.Status };

        _repoMock
            .Setup(m => m.AddAsync(dto, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new CreateOrder(_repoMock.Object);

        var result = await handler.CreateAsync(dto, CancellationToken.None);

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.AddAsync(dto, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateOrder_Forwards_ToRepository()
    {
        var orderId = Guid.NewGuid();
        var dto = new UpdateOrderDto { Status = "Shipped" };
        var expected = new OrderDto { Id = orderId, PersonId = Guid.NewGuid(), Status = dto.Status };

        _repoMock
            .Setup(m => m.UpdateAsync(orderId, dto, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new UpdateOrder(_repoMock.Object);

        var result = await handler.UpdateAsync(orderId, dto, CancellationToken.None);

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.UpdateAsync(orderId, dto, CancellationToken.None), Times.Once);
    }
}
