using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SolidApiExample.Api.Controllers;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.TestSuite.Controllers;

public sealed class OrdersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Fact]
    public async Task Get_ReturnsOrder_FromHandler()
    {
        var orderId = Guid.NewGuid();
        var expected = new OrderDto { Id = orderId, PersonId = Guid.NewGuid(), Status = OrderStatusDto.Pending };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetOrderQuery>(q => q.Id == orderId), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Get(orderId, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(m => m.Send(It.Is<GetOrderQuery>(q => q.Id == orderId), cancellation), Times.Once);
    }

    [Fact]
    public async Task List_ReturnsPagedResult_FromHandler()
    {
        const int page = 2;
        const int size = 5;
        var cancellation = CancellationToken.None;
        var orders = new List<OrderDto>
        {
            new() { Id = Guid.NewGuid(), PersonId = Guid.NewGuid(), Status = OrderStatusDto.Pending },
            new() { Id = Guid.NewGuid(), PersonId = Guid.NewGuid(), Status = OrderStatusDto.Completed }
        };
        var expected = new Paged<OrderDto>
        {
            Items = orders,
            Page = page,
            Size = size,
            Total = 42
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<ListOrdersQuery>(q => q.Page == page && q.Size == size), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.List(page, size, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(
            m => m.Send(It.Is<ListOrdersQuery>(q => q.Page == page && q.Size == size), cancellation),
            Times.Once);
    }

    [Fact]
    public async Task Create_ForwardsRequest_ToHandler()
    {
        var dto = new CreateOrderDto { PersonId = Guid.NewGuid(), Status = OrderStatusDto.Pending };
        var expected = new OrderDto { Id = Guid.NewGuid(), PersonId = dto.PersonId, Status = dto.Status };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<CreateOrderCommand>(c => c.Dto == dto), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Create(dto, cancellation);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(OrdersController.Get), created.ActionName);
        Assert.Equal(expected.Id, ((dynamic)created.RouteValues!)?["id"]);
        Assert.Same(expected, created.Value);
        _mediatorMock.Verify(m => m.Send(It.Is<CreateOrderCommand>(c => c.Dto == dto), cancellation), Times.Once);
    }

    [Fact]
    public async Task Update_ForwardsRequest_ToHandler()
    {
        var orderId = Guid.NewGuid();
        var dto = new UpdateOrderDto { Status = OrderStatusDto.Completed };
        var expected = new OrderDto { Id = orderId, PersonId = Guid.NewGuid(), Status = dto.Status };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<UpdateOrderCommand>(c => c.Id == orderId && c.Dto == dto), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Update(orderId, dto, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateOrderCommand>(c => c.Id == orderId && c.Dto == dto), cancellation),
            Times.Once);
    }

    private OrdersController CreateController() => new(_mediatorMock.Object);
}
