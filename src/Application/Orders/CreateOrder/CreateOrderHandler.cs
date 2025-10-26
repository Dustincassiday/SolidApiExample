using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Domain.Orders;
using SolidApiExample.Application.Orders;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrder : ICreate<CreateOrderDto, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public CreateOrder(IOrdersRepo repo) => _repo = repo;

    public async Task<OrderDto> CreateAsync(CreateOrderDto input, CancellationToken ct = default)
    {
        var status = input.Status.ToOrderStatus();
        var order = Order.Create(input.PersonId, status);
        var created = await _repo.AddAsync(order, ct);
        return created.ToDto();
    }
}
