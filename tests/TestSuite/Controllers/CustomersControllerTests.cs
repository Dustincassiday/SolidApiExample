using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SolidApiExample.Api.Controllers;
using SolidApiExample.Application.Customers.CreateCustomer;
using SolidApiExample.Application.Customers.DeleteCustomer;
using SolidApiExample.Application.Customers.GetCustomer;
using SolidApiExample.Application.Customers.ListCustomers;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Application.Customers.UpdateCustomer;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.TestSuite.Controllers;

public sealed class CustomersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Fact]
    public async Task Get_ReturnsCustomer_FromHandler()
    {
        var customerId = Guid.NewGuid();
        var expected = new CustomerDto { Id = customerId, Name = "Ada Lovelace" };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCustomerQuery>(q => q.Id == customerId), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Get(customerId, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(m => m.Send(It.Is<GetCustomerQuery>(q => q.Id == customerId), cancellation), Times.Once);
    }

    [Fact]
    public async Task List_ReturnsPagedResult_FromHandler()
    {
        const int page = 1;
        const int size = 10;
        var cancellation = CancellationToken.None;
        var customers = new List<CustomerDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Ada Lovelace" },
            new() { Id = Guid.NewGuid(), Name = "Alan Turing" }
        };
        var expected = new Paged<CustomerDto>
        {
            Items = customers,
            Page = page,
            Size = size,
            Total = 50
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<ListCustomersQuery>(q => q.Page == page && q.Size == size), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.List(page, size, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(
            m => m.Send(It.Is<ListCustomersQuery>(q => q.Page == page && q.Size == size), cancellation),
            Times.Once);
    }

    [Fact]
    public async Task Create_ForwardsRequest_ToHandler()
    {
        var dto = new CreateCustomerDto { Name = "Grace Hopper" };
        var expected = new CustomerDto { Id = Guid.NewGuid(), Name = dto.Name };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<CreateCustomerCommand>(c => c.Dto == dto), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Create(dto, cancellation);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(CustomersController.Get), created.ActionName);
        Assert.Equal(expected.Id, ((dynamic)created.RouteValues!)?["id"]);
        Assert.Same(expected, created.Value);
        _mediatorMock.Verify(m => m.Send(It.Is<CreateCustomerCommand>(c => c.Dto == dto), cancellation), Times.Once);
    }

    [Fact]
    public async Task Update_ForwardsRequest_ToHandler()
    {
        var customerId = Guid.NewGuid();
        var dto = new UpdateCustomerDto { Name = "Updated Name" };
        var expected = new CustomerDto { Id = customerId, Name = dto.Name };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<UpdateCustomerCommand>(c => c.Id == customerId && c.Dto == dto), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Update(customerId, dto, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateCustomerCommand>(c => c.Id == customerId && c.Dto == dto), cancellation),
            Times.Once);
    }

    [Fact]
    public async Task Delete_ForwardsRequest_ToHandler()
    {
        var customerId = Guid.NewGuid();
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteCustomerCommand>(c => c.Id == customerId), cancellation))
            .ReturnsAsync(Unit.Value);

        var controller = CreateController();

        var result = await controller.Delete(customerId, cancellation);

        Assert.IsType<NoContentResult>(result);
        _mediatorMock.Verify(m => m.Send(It.Is<DeleteCustomerCommand>(c => c.Id == customerId), cancellation), Times.Once);
    }

    private CustomersController CreateController() => new(_mediatorMock.Object);
}
