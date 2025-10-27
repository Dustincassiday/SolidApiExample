using MediatR;
using SolidApiExample.Application.Customers;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.Customers.GetCustomer;

public sealed record GetCustomerQuery(Guid Id) : IRequest<CustomerDto>;

public sealed class GetCustomerHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
{
    private readonly ICustomersRepo _repo;

    public GetCustomerHandler(ICustomersRepo repo) => _repo = repo;

    public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repo.FindAsync(request.Id, cancellationToken) ??
                     throw new KeyNotFoundException("Customer not found");
        return customer.ToDto();
    }
}
