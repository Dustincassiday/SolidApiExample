using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Infrastructure.Repositories.InMemory;
using Xunit;

namespace SolidApiExample.UnitTests.Infrastructure.Repositories;

public sealed class InMemoryOrdersRepoTests
{
    private readonly InMemoryOrdersRepo _repo = new();
    private readonly CancellationToken _ct = CancellationToken.None;

    [Fact]
    public async Task AddAsync_AssignsIdAndPersistsOrder()
    {
        var create = new CreateOrderDto { PersonId = Guid.NewGuid(), Status = "Pending" };

        var order = await _repo.AddAsync(create, _ct);

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(create.PersonId, order.PersonId);
        Assert.Equal(create.Status, order.Status);

        var found = await _repo.FindAsync(order.Id, _ct);
        Assert.Same(order, found);
    }

    [Fact]
    public async Task ListAsync_ReturnsPagedOrders()
    {
        await _repo.AddAsync(new CreateOrderDto { PersonId = Guid.NewGuid(), Status = "New" }, _ct);
        await _repo.AddAsync(new CreateOrderDto { PersonId = Guid.NewGuid(), Status = "Processing" }, _ct);

        var page = await _repo.ListAsync(page: 0, size: 1, _ct);

        Assert.Equal(0, page.Page);
        Assert.Equal(1, page.Size);
        Assert.Equal(2, page.Total);
        Assert.Single(page.Items);
    }

    [Fact]
    public async Task UpdateAsync_ChangesExistingOrder()
    {
        var order = await _repo.AddAsync(new CreateOrderDto { PersonId = Guid.NewGuid(), Status = "Pending" }, _ct);
        var update = new UpdateOrderDto { Status = "Completed" };

        var updated = await _repo.UpdateAsync(order.Id, update, _ct);

        Assert.Equal(order.Id, updated.Id);
        Assert.Equal("Completed", updated.Status);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenOrderMissing()
    {
        var update = new UpdateOrderDto { Status = "Completed" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repo.UpdateAsync(Guid.NewGuid(), update, _ct));
    }
}
