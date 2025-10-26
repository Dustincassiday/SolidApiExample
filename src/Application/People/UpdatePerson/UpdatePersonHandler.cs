using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.UpdatePerson;

public sealed class UpdatePerson : IUpdate<Guid, UpdatePersonDto, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public UpdatePerson(IPeopleRepo repo) => _repo = repo;

    public async Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto input, CancellationToken ct = default)
    {
        var name = input.Name.ValidateAndNormalizeName();
        var updated = await _repo.UpdateNameAsync(id, name, ct);
        return updated.ToDto();
    }
}
