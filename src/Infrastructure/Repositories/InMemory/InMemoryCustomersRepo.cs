using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.Customers;
using SolidApiExample.Domain.Shared;

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
            .Select(p => Customer.FromExisting(p.Id, p.Name, p.Email))
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
        var stored = Customer.FromExisting(customer.Id, customer.Name, customer.Email);
        _customers.Add(stored);
        return Task.FromResult(stored);
    }

    public async Task<Customer> UpdateAsync(Guid id, string name, Email email, CancellationToken ct)
    {
        var customer = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Customer not found");
        customer.Rename(name);
        customer.UpdateEmail(email);
        return customer;
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var customer = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Customer not found");
        _customers.Remove(customer);
    }
}
