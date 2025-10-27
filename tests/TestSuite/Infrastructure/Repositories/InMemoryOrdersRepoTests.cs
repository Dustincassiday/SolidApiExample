using SolidApiExample.Infrastructure.Repositories.InMemory;
using SolidApiExample.Domain.Orders;
using SolidApiExample.Domain.Shared;

namespace SolidApiExample.TestSuite.Infrastructure.Repositories;

public sealed class InMemoryOrdersRepoTests
{
    private readonly InMemoryOrdersRepo _repo = new();
    private readonly CancellationToken _ct = CancellationToken.None;

    [Fact]
    public async Task AddAsync_AssignsIdAndPersistsOrder()
    {
        var create = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));

        var order = await _repo.AddAsync(create, _ct);

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(create.CustomerId, order.CustomerId);
        Assert.Equal(create.Status, order.Status);
        Assert.Equal(create.Total, order.Total);

        var found = await _repo.FindAsync(order.Id, _ct);
        Assert.NotNull(found);
        Assert.NotSame(order, found);
        Assert.Equal(order.Id, found!.Id);
        Assert.Equal(order.CustomerId, found.CustomerId);
        Assert.Equal(order.Status, found.Status);
        Assert.Equal(order.Total, found.Total);
    }

    [Fact]
    public async Task ListAsync_ReturnsPagedOrders()
    {
        await _repo.AddAsync(Order.Create(Guid.NewGuid(), Money.Create(10m, "USD")), _ct);
        await _repo.AddAsync(Order.Create(Guid.NewGuid(), Money.Create(15m, "USD")), _ct);

        var page = await _repo.ListAsync(page: 0, size: 1, _ct);

        Assert.Equal(0, page.Page);
        Assert.Equal(1, page.Size);
        Assert.Equal(2, page.Total);
        Assert.Single(page.Items);
    }

    [Fact]
    public async Task UpdateAsync_ChangesExistingOrder()
    {
        var order = await _repo.AddAsync(Order.Create(Guid.NewGuid(), Money.Create(10m, "USD")), _ct);

        var updated = await _repo.UpdateStatusAsync(order.Id, OrderStatus.Paid, _ct);

        Assert.Equal(order.Id, updated.Id);
        Assert.Equal(OrderStatus.Paid, updated.Status);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenOrderMissing()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repo.UpdateStatusAsync(Guid.NewGuid(), OrderStatus.Paid, _ct));
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenTransitionInvalid()
    {
        var order = await _repo.AddAsync(Order.Create(Guid.NewGuid(), Money.Create(10m, "USD")), _ct);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repo.UpdateStatusAsync(order.Id, OrderStatus.Shipped, _ct));
    }

    [Fact]
    public async Task AddAsync_Throws_WhenDuplicateId()
    {
        var order = await _repo.AddAsync(Order.Create(Guid.NewGuid(), Money.Create(10m, "USD")), _ct);
        var duplicate = Order.FromExisting(order.Id, order.CustomerId, order.Status, order.Total);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _repo.AddAsync(duplicate, _ct));
    }
}
