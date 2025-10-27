using MediatR;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed record UpdateOrderCommand(Guid Id, UpdateOrderDto Dto) : IRequest<OrderDto>;

public sealed class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
{
    private readonly IOrdersRepo _repo;

    public UpdateOrderHandler(IOrdersRepo repo) => _repo = repo;

    public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var updated = await _repo.UpdateStatusAsync(
            request.Id,
            request.Dto.Status.ToDomain(),
            cancellationToken);
        return updated.ToDto();
    }
}
