using SolidApiExample.Infrastructure.Repositories.InMemory;
using SolidApiExample.Domain.Orders;

namespace SolidApiExample.TestSuite.Infrastructure.Repositories;

public sealed class InMemoryOrdersRepoTests
{
    private readonly InMemoryOrdersRepo _repo = new();
    private readonly CancellationToken _ct = CancellationToken.None;

    [Fact]
    public async Task AddAsync_AssignsIdAndPersistsOrder()
    {
        var create = Order.Create(Guid.NewGuid(), OrderStatus.Pending);

        var order = await _repo.AddAsync(create, _ct);

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(create.CustomerId, order.CustomerId);
        Assert.Equal(create.Status, order.Status);

        var found = await _repo.FindAsync(order.Id, _ct);
        Assert.Same(order, found);
    }

    [Fact]
    public async Task ListAsync_ReturnsPagedOrders()
    {
        await _repo.AddAsync(Order.Create(Guid.NewGuid(), OrderStatus.Pending), _ct);
        await _repo.AddAsync(Order.Create(Guid.NewGuid(), OrderStatus.Processing), _ct);

        var page = await _repo.ListAsync(page: 0, size: 1, _ct);

        Assert.Equal(0, page.Page);
        Assert.Equal(1, page.Size);
        Assert.Equal(2, page.Total);
        Assert.Single(page.Items);
    }

    [Fact]
    public async Task UpdateAsync_ChangesExistingOrder()
    {
        var order = await _repo.AddAsync(Order.Create(Guid.NewGuid(), OrderStatus.Pending), _ct);

        var updated = await _repo.UpdateStatusAsync(order.Id, OrderStatus.Completed, _ct);

        Assert.Equal(order.Id, updated.Id);
        Assert.Equal(OrderStatus.Completed, updated.Status);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenOrderMissing()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repo.UpdateStatusAsync(Guid.NewGuid(), OrderStatus.Completed, _ct));
    }
}
