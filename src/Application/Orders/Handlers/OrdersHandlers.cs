using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.DTOs;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.Orders.Handlers;

public sealed class GetOrder : IGetById<Guid, OrderDto>
{
    private readonly IOrdersRepo _repo; public GetOrder(IOrdersRepo repo) => _repo = repo;
    public async Task<OrderDto> GetAsync(Guid id, CancellationToken ct = default) =>
        await _repo.FindAsync(id, ct) ?? throw new System.Collections.Generic.KeyNotFoundException("Order not found");
}
public sealed class ListOrders : IListItems<OrderDto>
{
    private readonly IOrdersRepo _repo; public ListOrders(IOrdersRepo repo) => _repo = repo;
    public Task<Paged<OrderDto>> ListAsync(int page, int size, CancellationToken ct = default) => _repo.ListAsync(page, size, ct);
}
public sealed class CreateOrder : ICreate<CreateOrderDto, OrderDto>
{
    private readonly IOrdersRepo _repo; public CreateOrder(IOrdersRepo repo) => _repo = repo;
    public Task<OrderDto> CreateAsync(CreateOrderDto input, CancellationToken ct = default) => _repo.AddAsync(input, ct);
}
public sealed class UpdateOrder : IUpdate<Guid, UpdateOrderDto, OrderDto>
{
    private readonly IOrdersRepo _repo; public UpdateOrder(IOrdersRepo repo) => _repo = repo;
    public Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input, CancellationToken ct = default) => _repo.UpdateAsync(id, input, ct);
}
