using MediatR;
using Moq;
using SolidApiExample.Application.Customers.CreateCustomer;
using SolidApiExample.Application.Customers.DeleteCustomer;
using SolidApiExample.Application.Customers.GetCustomer;
using SolidApiExample.Application.Customers.ListCustomers;
using SolidApiExample.Application.Customers.UpdateCustomer;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Customers;

namespace SolidApiExample.TestSuite.Application.Customers;

public sealed class CustomersHandlersTests
{
    private readonly Mock<ICustomersRepo> _repoMock = new();

    [Fact]
    public async Task GetCustomer_Returns_Customer_WhenFound()
    {
        var customerId = Guid.NewGuid();
        var expected = Customer.FromExisting(customerId, "Ada Lovelace");

        _repoMock
            .Setup(m => m.FindAsync(customerId, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new GetCustomerHandler(_repoMock.Object);

        var result = await handler.Handle(new GetCustomerQuery(customerId), CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.Name, result.Name);
        _repoMock.Verify(m => m.FindAsync(customerId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetCustomer_Throws_WhenNotFound()
    {
        var customerId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.FindAsync(customerId, CancellationToken.None))
            .ReturnsAsync((Customer?)null);

        var handler = new GetCustomerHandler(_repoMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new GetCustomerQuery(customerId), CancellationToken.None));
        _repoMock.Verify(m => m.FindAsync(customerId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ListCustomers_Returns_PagedCustomers()
    {
        const int page = 0;
        const int size = 20;
        var expected = new Paged<Customer>
        {
            Items = new List<Customer>
            {
                Customer.FromExisting(Guid.NewGuid(), "Grace Hopper")
            },
            Page = page,
            Size = size,
            Total = 1
        };

        _repoMock
            .Setup(m => m.ListAsync(page, size, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new ListCustomersHandler(_repoMock.Object);

        var result = await handler.Handle(new ListCustomersQuery(page, size), CancellationToken.None);

        Assert.Equal(expected.Total, result.Total);
        Assert.Equal(expected.Page, result.Page);
        Assert.Equal(expected.Size, result.Size);
        Assert.Single(result.Items);
        Assert.Equal(expected.Items.First().Name, result.Items.First().Name);
        _repoMock.Verify(m => m.ListAsync(page, size, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateCustomer_Forwards_ToRepository()
    {
        var dto = new CreateCustomerDto { Name = "Alan Turing" };
        _repoMock
            .Setup(m => m.AddAsync(It.IsAny<Customer>(), CancellationToken.None))
            .ReturnsAsync((Customer p, CancellationToken _) => Customer.FromExisting(p.Id, p.Name));

        var handler = new CreateCustomerHandler(_repoMock.Object);

        var result = await handler.Handle(new CreateCustomerCommand(dto), CancellationToken.None);

        Assert.Equal(dto.Name, result.Name);
        _repoMock.Verify(m => m.AddAsync(It.Is<Customer>(p => p.Name == dto.Name), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomer_Forwards_ToRepository()
    {
        var customerId = Guid.NewGuid();
        var dto = new UpdateCustomerDto { Name = "Updated Name" };
        var expected = Customer.FromExisting(customerId, dto.Name);

        _repoMock
            .Setup(m => m.UpdateNameAsync(customerId, dto.Name, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new UpdateCustomerHandler(_repoMock.Object);

        var result = await handler.Handle(new UpdateCustomerCommand(customerId, dto), CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.Name, result.Name);
        _repoMock.Verify(m => m.UpdateNameAsync(customerId, dto.Name, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomer_Forwards_ToRepository()
    {
        var customerId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.DeleteAsync(customerId, CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new DeleteCustomerHandler(_repoMock.Object);

        var result = await handler.Handle(new DeleteCustomerCommand(customerId), CancellationToken.None);

        Assert.Equal(Unit.Value, result);
        _repoMock.Verify(m => m.DeleteAsync(customerId, CancellationToken.None), Times.Once);
    }
}
