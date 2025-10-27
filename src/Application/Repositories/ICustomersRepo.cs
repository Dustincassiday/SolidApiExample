using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Customers;

namespace SolidApiExample.Application.Repositories;

public interface ICustomersRepo
{
    Task<Customer?> FindAsync(Guid id, CancellationToken ct);
    Task<Paged<Customer>> ListAsync(int page, int size, CancellationToken ct);
    Task<Customer> AddAsync(Customer customer, CancellationToken ct);
    Task<Customer> UpdateNameAsync(Guid id, string name, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
