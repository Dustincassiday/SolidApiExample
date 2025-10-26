using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Orders.DTOs;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.Repositories;

public interface IOrdersRepo
{
    Task<OrderDto?> FindAsync(Guid id, CancellationToken ct);
    Task<Paged<OrderDto>> ListAsync(int page, int size, CancellationToken ct);
    Task<OrderDto> AddAsync(CreateOrderDto dto, CancellationToken ct);
    Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto dto, CancellationToken ct);
}
