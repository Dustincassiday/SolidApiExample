using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.DeletePerson;

public sealed class DeletePerson : IDelete<Guid>
{
    private readonly IPeopleRepo _repo;

    public DeletePerson(IPeopleRepo repo) => _repo = repo;

    public Task DeleteAsync(Guid id, CancellationToken ct = default) =>
        _repo.DeleteAsync(id, ct);
}
