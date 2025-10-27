using MediatR;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Domain.Orders;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed record CreateOrderCommand(CreateOrderDto Dto) : IRequest<OrderDto>;

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public CreateOrderHandler(IOrdersRepo repo) => _repo = repo;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.Dto.PersonId, request.Dto.Status.ToDomain());
        var created = await _repo.AddAsync(order, cancellationToken);
        return created.ToDto();
    }
}
