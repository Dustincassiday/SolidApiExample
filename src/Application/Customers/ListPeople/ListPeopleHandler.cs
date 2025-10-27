using MediatR;
using SolidApiExample.Application.Customers;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.Customers.ListCustomers;

public sealed record ListCustomersQuery(int Page, int Size) : IRequest<Paged<CustomerDto>>;

public sealed class ListCustomersHandler : IRequestHandler<ListCustomersQuery, Paged<CustomerDto>>
{
    private readonly ICustomersRepo _repo;

    public ListCustomersHandler(ICustomersRepo repo) => _repo = repo;

    public async Task<Paged<CustomerDto>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _repo.ListAsync(request.Page, request.Size, cancellationToken);
        return customers.ToDto();
    }
}
