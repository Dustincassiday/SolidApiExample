using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.CreatePerson;

public sealed class CreatePerson : ICreate<CreatePersonDto, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public CreatePerson(IPeopleRepo repo) => _repo = repo;

    public async Task<PersonDto> CreateAsync(CreatePersonDto input, CancellationToken ct = default)
    {
        var person = input.ToPerson();
        var created = await _repo.AddAsync(person, ct);
        return created.ToDto();
    }
}
