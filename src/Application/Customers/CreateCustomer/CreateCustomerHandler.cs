using MediatR;
using SolidApiExample.Application.Customers;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Customers.CreateCustomer;

public sealed record CreateCustomerCommand(CreateCustomerDto Dto) : IRequest<CustomerDto>;

public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomersRepo _repo;

    public CreateCustomerHandler(ICustomersRepo repo) => _repo = repo;

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = request.Dto.ToCustomer();
        var created = await _repo.AddAsync(customer, cancellationToken);
        return created.ToDto();
    }
}
