using MediatR;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Customers.DeleteCustomer;

public sealed record DeleteCustomerCommand(Guid Id) : IRequest<Unit>;

public sealed class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, Unit>
{
    private readonly ICustomersRepo _repo;

    public DeleteCustomerHandler(ICustomersRepo repo) => _repo = repo;

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
