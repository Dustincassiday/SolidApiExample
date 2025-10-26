using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Orders.DTOs;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Infrastructure.Repositories.InMemory;

public sealed class InMemoryOrdersRepo : IOrdersRepo
{
    private readonly List<OrderDto> _orders = new();
    public Task<OrderDto?> FindAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
    public Task<Paged<OrderDto>> ListAsync(int page, int size, CancellationToken ct)
    {
        var items = _orders.Skip(page * size).Take(size).ToList();
        return Task.FromResult(new Paged<OrderDto> { Items = items, Page = page, Size = size, Total = _orders.Count });
    }
    public Task<OrderDto> AddAsync(CreateOrderDto dto, CancellationToken ct)
    {
        var o = new OrderDto { Id = Guid.NewGuid(), PersonId = dto.PersonId, Status = dto.Status };
        _orders.Add(o); return Task.FromResult(o);
    }
    public async Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto dto, CancellationToken ct)
    {
        var o = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Order not found");
        o.Status = dto.Status; return o;
    }
}
