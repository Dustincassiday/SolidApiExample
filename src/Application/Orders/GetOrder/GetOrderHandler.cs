using MediatR;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Orders.GetOrder;

public sealed record GetOrderQuery(Guid Id) : IRequest<OrderDto>;

public sealed class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public GetOrderHandler(IOrdersRepo repo) => _repo = repo;

    public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _repo.FindAsync(request.Id, cancellationToken) ??
            throw new KeyNotFoundException("Order not found");
        return order.ToDto();
    }
}
