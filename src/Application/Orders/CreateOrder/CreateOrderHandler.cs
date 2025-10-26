using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrder : ICreate<CreateOrderDto, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public CreateOrder(IOrdersRepo repo) => _repo = repo;

    public Task<OrderDto> CreateAsync(CreateOrderDto input, CancellationToken ct = default) =>
        _repo.AddAsync(input, ct);
}
