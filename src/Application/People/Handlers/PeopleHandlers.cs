using System;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.DTOs;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.People.Handlers;

public sealed class GetPerson : IGetById<Guid, PersonDto>
{
    private readonly IPeopleRepo _repo; public GetPerson(IPeopleRepo repo) => _repo = repo;
    public async Task<PersonDto> GetAsync(Guid id, CancellationToken ct = default) =>
        await _repo.FindAsync(id, ct) ?? throw new System.Collections.Generic.KeyNotFoundException("Person not found");
}
public sealed class ListPeople : IListItems<PersonDto>
{
    private readonly IPeopleRepo _repo; public ListPeople(IPeopleRepo repo) => _repo = repo;
    public Task<Paged<PersonDto>> ListAsync(int page, int size, CancellationToken ct = default) => _repo.ListAsync(page, size, ct);
}
public sealed class CreatePerson : ICreate<CreatePersonDto, PersonDto>
{
    private readonly IPeopleRepo _repo; public CreatePerson(IPeopleRepo repo) => _repo = repo;
    public Task<PersonDto> CreateAsync(CreatePersonDto input, CancellationToken ct = default) => _repo.AddAsync(input, ct);
}
public sealed class UpdatePerson : IUpdate<Guid, UpdatePersonDto, PersonDto>
{
    private readonly IPeopleRepo _repo; public UpdatePerson(IPeopleRepo repo) => _repo = repo;
    public Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto input, CancellationToken ct = default) => _repo.UpdateAsync(id, input, ct);
}
public sealed class DeletePerson : IDelete<Guid>
{
    private readonly IPeopleRepo _repo; public DeletePerson(IPeopleRepo repo) => _repo = repo;
    public Task DeleteAsync(Guid id, CancellationToken ct = default) => _repo.DeleteAsync(id, ct);
}
