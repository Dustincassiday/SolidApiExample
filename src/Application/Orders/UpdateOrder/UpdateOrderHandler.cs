using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrder : IUpdate<Guid, UpdateOrderDto, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public UpdateOrder(IOrdersRepo repo) => _repo = repo;

    public async Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input, CancellationToken ct = default)
    {
        var status = input.Status.ToOrderStatus();
        var updated = await _repo.UpdateStatusAsync(id, status, ct);
        return updated.ToDto();
    }
}
