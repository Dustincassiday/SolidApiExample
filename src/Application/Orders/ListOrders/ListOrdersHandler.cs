using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.Orders.ListOrders;

public sealed class ListOrders : IListItems<OrderDto>
{
    private readonly IOrdersRepo _repo;

    public ListOrders(IOrdersRepo repo) => _repo = repo;

    public async Task<Paged<OrderDto>> ListAsync(int page, int size, CancellationToken ct = default)
    {
        var orders = await _repo.ListAsync(page, size, ct);
        return orders.ToDto();
    }
}
