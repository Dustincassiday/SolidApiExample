using SolidApiExample.Infrastructure.Repositories.InMemory;
using SolidApiExample.Domain.People;


namespace SolidApiExample.UnitTests.Infrastructure.Repositories;

public sealed class InMemoryPeopleRepoTests
{
    private readonly InMemoryPeopleRepo _repo = new();
    private readonly CancellationToken _ct = CancellationToken.None;

    [Fact]
    public async Task AddAsync_AssignsIdAndPersistsPerson()
    {
        var create = Person.Create("Ada Lovelace");

        var person = await _repo.AddAsync(create, _ct);

        Assert.NotEqual(Guid.Empty, person.Id);
        Assert.Equal(create.Name, person.Name);

        var found = await _repo.FindAsync(person.Id, _ct);
        Assert.Same(person, found);
    }

    [Fact]
    public async Task ListAsync_ReturnsPagedPeople()
    {
        await _repo.AddAsync(Person.Create("Ada Lovelace"), _ct);
        await _repo.AddAsync(Person.Create("Grace Hopper"), _ct);

        var page = await _repo.ListAsync(page: 0, size: 1, _ct);

        Assert.Equal(0, page.Page);
        Assert.Equal(1, page.Size);
        Assert.Equal(2, page.Total);
        Assert.Single(page.Items);
    }

    [Fact]
    public async Task UpdateAsync_ChangesExistingPerson()
    {
        var person = await _repo.AddAsync(Person.Create("Ada Lovelace"), _ct);
        var updateName = "Ada King";

        var updated = await _repo.UpdateNameAsync(person.Id, updateName, _ct);

        Assert.Equal(person.Id, updated.Id);
        Assert.Equal("Ada King", updated.Name);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenPersonMissing()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repo.UpdateNameAsync(Guid.NewGuid(), "Alan Turing", _ct));
    }

    [Fact]
    public async Task DeleteAsync_RemovesPerson()
    {
        var person = await _repo.AddAsync(Person.Create("Grace Hopper"), _ct);

        await _repo.DeleteAsync(person.Id, _ct);

        var found = await _repo.FindAsync(person.Id, _ct);
        Assert.Null(found);
    }
}
