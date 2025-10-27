using SolidApiExample.Infrastructure.Repositories.InMemory;
using SolidApiExample.Domain.Customers;


namespace SolidApiExample.TestSuite.Infrastructure.Repositories;

public sealed class InMemoryCustomersRepoTests
{
    private readonly InMemoryCustomersRepo _repo = new();
    private readonly CancellationToken _ct = CancellationToken.None;

    [Fact]
    public async Task AddAsync_AssignsIdAndPersistsCustomer()
    {
        var create = Customer.Create("Ada Lovelace");

        var customer = await _repo.AddAsync(create, _ct);

        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.Equal(create.Name, customer.Name);

        var found = await _repo.FindAsync(customer.Id, _ct);
        Assert.Same(customer, found);
    }

    [Fact]
    public async Task ListAsync_ReturnsPagedCustomers()
    {
        await _repo.AddAsync(Customer.Create("Ada Lovelace"), _ct);
        await _repo.AddAsync(Customer.Create("Grace Hopper"), _ct);

        var page = await _repo.ListAsync(page: 0, size: 1, _ct);

        Assert.Equal(0, page.Page);
        Assert.Equal(1, page.Size);
        Assert.Equal(2, page.Total);
        Assert.Single(page.Items);
    }

    [Fact]
    public async Task UpdateAsync_ChangesExistingCustomer()
    {
        var customer = await _repo.AddAsync(Customer.Create("Ada Lovelace"), _ct);
        var updateName = "Ada King";

        var updated = await _repo.UpdateNameAsync(customer.Id, updateName, _ct);

        Assert.Equal(customer.Id, updated.Id);
        Assert.Equal("Ada King", updated.Name);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenCustomerMissing()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repo.UpdateNameAsync(Guid.NewGuid(), "Alan Turing", _ct));
    }

    [Fact]
    public async Task DeleteAsync_RemovesCustomer()
    {
        var customer = await _repo.AddAsync(Customer.Create("Grace Hopper"), _ct);

        await _repo.DeleteAsync(customer.Id, _ct);

        var found = await _repo.FindAsync(customer.Id, _ct);
        Assert.Null(found);
    }
}
