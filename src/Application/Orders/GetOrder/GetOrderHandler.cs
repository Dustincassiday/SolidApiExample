using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Orders.GetOrder;

public sealed class GetOrder : IGetById<Guid, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public GetOrder(IOrdersRepo repo) => _repo = repo;

    public async Task<OrderDto> GetAsync(Guid id, CancellationToken ct = default) =>
        await _repo.FindAsync(id, ct) ?? throw new KeyNotFoundException("Order not found");
}
