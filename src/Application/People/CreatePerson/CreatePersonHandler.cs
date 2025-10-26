using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.CreatePerson;

public sealed class CreatePerson : ICreate<CreatePersonDto, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public CreatePerson(IPeopleRepo repo) => _repo = repo;

    public Task<PersonDto> CreateAsync(CreatePersonDto input, CancellationToken ct = default) =>
        _repo.AddAsync(input, ct);
}
