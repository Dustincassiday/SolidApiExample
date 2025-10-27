using MediatR;
using SolidApiExample.Application.Customers;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(Guid Id, UpdateCustomerDto Dto) : IRequest<CustomerDto>;

public sealed class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomersRepo _repo;

    public UpdateCustomerHandler(ICustomersRepo repo) => _repo = repo;

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var name = request.Dto.Name.ValidateAndNormalizeName();
        var updated = await _repo.UpdateNameAsync(request.Id, name, cancellationToken);
        return updated.ToDto();
    }
}
