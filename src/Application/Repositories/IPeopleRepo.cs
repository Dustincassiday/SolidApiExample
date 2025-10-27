using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.People;

namespace SolidApiExample.Application.Repositories;

public interface IPeopleRepo
{
    Task<Person?> FindAsync(Guid id, CancellationToken ct);
    Task<Paged<Person>> ListAsync(int page, int size, CancellationToken ct);
    Task<Person> AddAsync(Person person, CancellationToken ct);
    Task<Person> UpdateNameAsync(Guid id, string name, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
