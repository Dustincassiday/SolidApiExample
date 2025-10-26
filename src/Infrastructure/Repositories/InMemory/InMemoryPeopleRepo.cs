using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.People;

namespace SolidApiExample.Infrastructure.Repositories.InMemory;

public sealed class InMemoryPeopleRepo : IPeopleRepo
{
    private readonly List<Person> _people = new();

    public Task<Person?> FindAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_people.FirstOrDefault(p => p.Id == id));

    public Task<Paged<Person>> ListAsync(int page, int size, CancellationToken ct)
    {
        var items = _people
            .Skip(page * size)
            .Take(size)
            .Select(p => Person.FromExisting(p.Id, p.Name))
            .ToList();

        return Task.FromResult(new Paged<Person>
        {
            Items = items,
            Page = page,
            Size = size,
            Total = _people.Count
        });
    }

    public Task<Person> AddAsync(Person person, CancellationToken ct)
    {
        var stored = Person.FromExisting(person.Id, person.Name);
        _people.Add(stored);
        return Task.FromResult(stored);
    }

    public async Task<Person> UpdateNameAsync(Guid id, string name, CancellationToken ct)
    {
        var person = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Person not found");
        person.Rename(name);
        return person;
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var person = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Person not found");
        _people.Remove(person);
    }
}
