using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Customers;

namespace SolidApiExample.Infrastructure.Repositories.InMemory;

public sealed class InMemoryCustomersRepo : ICustomersRepo
{
    private readonly List<Customer> _customers = new();

    public Task<Customer?> FindAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_customers.FirstOrDefault(p => p.Id == id));

    public Task<Paged<Customer>> ListAsync(int page, int size, CancellationToken ct)
    {
        var items = _customers
            .Skip(page * size)
            .Take(size)
            .Select(p => Customer.FromExisting(p.Id, p.Name))
            .ToList();

        return Task.FromResult(new Paged<Customer>
        {
            Items = items,
            Page = page,
            Size = size,
            Total = _customers.Count
        });
    }

    public Task<Customer> AddAsync(Customer customer, CancellationToken ct)
    {
        var stored = Customer.FromExisting(customer.Id, customer.Name);
        _customers.Add(stored);
        return Task.FromResult(stored);
    }

    public async Task<Customer> UpdateNameAsync(Guid id, string name, CancellationToken ct)
    {
        var customer = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Customer not found");
        customer.Rename(name);
        return customer;
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var customer = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Customer not found");
        _customers.Remove(customer);
    }
}
