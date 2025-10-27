using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrder : IUpdate<Guid, UpdateOrderDto, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public UpdateOrder(IOrdersRepo repo) => _repo = repo;

    public async Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input, CancellationToken ct = default)
    {
        var updated = await _repo.UpdateStatusAsync(id, input.Status.ToDomain(), ct);
        return updated.ToDto();
    }
}
