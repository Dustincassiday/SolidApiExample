using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrder : IUpdate<Guid, UpdateOrderDto, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public UpdateOrder(IOrdersRepo repo) => _repo = repo;

    public Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input, CancellationToken ct = default) =>
        _repo.UpdateAsync(id, input, ct);
}
