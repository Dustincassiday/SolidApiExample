using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Orders;

namespace SolidApiExample.Infrastructure.Repositories.InMemory;

public sealed class InMemoryOrdersRepo : IOrdersRepo
{
    private readonly List<Order> _orders = new();

    public Task<Order?> FindAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

    public Task<Paged<Order>> ListAsync(int page, int size, CancellationToken ct)
    {
        var items = _orders
            .Skip(page * size)
            .Take(size)
            .Select(o => Order.FromExisting(o.Id, o.CustomerId, o.Status))
            .ToList();

        return Task.FromResult(new Paged<Order>
        {
            Items = items,
            Page = page,
            Size = size,
            Total = _orders.Count
        });
    }

    public Task<Order> AddAsync(Order order, CancellationToken ct)
    {
        var stored = Order.FromExisting(order.Id, order.CustomerId, order.Status);
        _orders.Add(stored);
        return Task.FromResult(stored);
    }

    public async Task<Order> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken ct)
    {
        var order = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Order not found");
        order.UpdateStatus(status);
        return order;
    }
}
