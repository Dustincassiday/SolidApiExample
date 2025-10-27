using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Orders;

namespace SolidApiExample.Application.Repositories;

public interface IOrdersRepo
{
    Task<Order?> FindAsync(Guid id, CancellationToken ct);
    Task<Paged<Order>> ListAsync(int page, int size, CancellationToken ct);
    Task<Order> AddAsync(Order order, CancellationToken ct);
    Task<Order> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken ct);
}
