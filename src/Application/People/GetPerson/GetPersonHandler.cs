using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.GetPerson;

public sealed class GetPerson : IGetById<Guid, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public GetPerson(IPeopleRepo repo) => _repo = repo;

    public async Task<PersonDto> GetAsync(Guid id, CancellationToken ct = default)
    {
        var person = await _repo.FindAsync(id, ct) ?? throw new KeyNotFoundException("Person not found");
        return person.ToDto();
    }
}
