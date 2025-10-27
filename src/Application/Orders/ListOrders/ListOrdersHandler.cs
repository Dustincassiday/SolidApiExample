using MediatR;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.Orders.ListOrders;

public sealed record ListOrdersQuery(int Page, int Size) : IRequest<Paged<OrderDto>>;

public sealed class ListOrdersHandler : IRequestHandler<ListOrdersQuery, Paged<OrderDto>>
{
    private readonly IOrdersRepo _repo;

    public ListOrdersHandler(IOrdersRepo repo) => _repo = repo;

    public async Task<Paged<OrderDto>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repo.ListAsync(request.Page, request.Size, cancellationToken);
        return orders.ToDto();
    }
}
