using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.UpdatePerson;

public sealed class UpdatePerson : IUpdate<Guid, UpdatePersonDto, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public UpdatePerson(IPeopleRepo repo) => _repo = repo;

    public Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto input, CancellationToken ct = default) =>
        _repo.UpdateAsync(id, input, ct);
}
