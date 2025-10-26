using Microsoft.AspNetCore.Mvc;
using Moq;
using SolidApiExample.Api.Controllers;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.UnitTests.Controllers;

public sealed class OrdersControllerTests
{
    private readonly Mock<IGetById<Guid, OrderDto>> _getMock = new();
    private readonly Mock<IListItems<OrderDto>> _listMock = new();
    private readonly Mock<ICreate<CreateOrderDto, OrderDto>> _createMock = new();
    private readonly Mock<IUpdate<Guid, UpdateOrderDto, OrderDto>> _updateMock = new();

    [Fact]
    public async Task Get_ReturnsOrder_FromHandler()
    {
        var orderId = Guid.NewGuid();
        var expected = new OrderDto { Id = orderId, PersonId = Guid.NewGuid(), Status = "Pending" };
        var cancellation = CancellationToken.None;

        _getMock
            .Setup(m => m.GetAsync(orderId, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Get(orderId, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _getMock.Verify(m => m.GetAsync(orderId, cancellation), Times.Once);
    }

    [Fact]
    public async Task List_ReturnsPagedResult_FromHandler()
    {
        const int page = 2;
        const int size = 5;
        var cancellation = CancellationToken.None;
        var orders = new List<OrderDto>
        {
            new() { Id = Guid.NewGuid(), PersonId = Guid.NewGuid(), Status = "Pending" },
            new() { Id = Guid.NewGuid(), PersonId = Guid.NewGuid(), Status = "Shipped" }
        };
        var expected = new Paged<OrderDto>
        {
            Items = orders,
            Page = page,
            Size = size,
            Total = 42
        };

        _listMock
            .Setup(m => m.ListAsync(page, size, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.List(page, size, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _listMock.Verify(m => m.ListAsync(page, size, cancellation), Times.Once);
    }

    [Fact]
    public async Task Create_ForwardsRequest_ToHandler()
    {
        var dto = new CreateOrderDto { PersonId = Guid.NewGuid(), Status = "Pending" };
        var expected = new OrderDto { Id = Guid.NewGuid(), PersonId = dto.PersonId, Status = dto.Status };
        var cancellation = CancellationToken.None;

        _createMock
            .Setup(m => m.CreateAsync(dto, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Create(dto, cancellation);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(OrdersController.Get), created.ActionName);
        Assert.Equal(expected.Id, ((dynamic)created.RouteValues!)?["id"]);
        Assert.Same(expected, created.Value);
        _createMock.Verify(m => m.CreateAsync(dto, cancellation), Times.Once);
    }

    [Fact]
    public async Task Update_ForwardsRequest_ToHandler()
    {
        var orderId = Guid.NewGuid();
        var dto = new UpdateOrderDto { Status = "Completed" };
        var expected = new OrderDto { Id = orderId, PersonId = Guid.NewGuid(), Status = dto.Status };
        var cancellation = CancellationToken.None;

        _updateMock
            .Setup(m => m.UpdateAsync(orderId, dto, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Update(orderId, dto, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _updateMock.Verify(m => m.UpdateAsync(orderId, dto, cancellation), Times.Once);
    }

    private OrdersController CreateController() =>
        new(_getMock.Object, _listMock.Object, _createMock.Object, _updateMock.Object);
}
