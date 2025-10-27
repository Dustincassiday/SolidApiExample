using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Customers;
using SolidApiExample.Domain.Shared;

namespace SolidApiExample.Application.Repositories;

public interface ICustomersRepo
{
    Task<Customer?> FindAsync(Guid id, CancellationToken ct);
    Task<Paged<Customer>> ListAsync(int page, int size, CancellationToken ct);
    Task<Customer> AddAsync(Customer customer, CancellationToken ct);
    Task<Customer> UpdateAsync(Guid id, string name, Email email, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
