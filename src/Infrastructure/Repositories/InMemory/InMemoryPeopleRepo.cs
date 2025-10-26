using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.People.DTOs;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Infrastructure.Repositories.InMemory;

public sealed class InMemoryPeopleRepo : IPeopleRepo
{
    private readonly List<PersonDto> _people = new();
    public Task<PersonDto?> FindAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_people.FirstOrDefault(p => p.Id == id));
    public Task<Paged<PersonDto>> ListAsync(int page, int size, CancellationToken ct)
    {
        var items = _people.Skip(page * size).Take(size).ToList();
        return Task.FromResult(new Paged<PersonDto> { Items = items, Page = page, Size = size, Total = _people.Count });
    }
    public Task<PersonDto> AddAsync(CreatePersonDto dto, CancellationToken ct)
    {
        var p = new PersonDto { Id = Guid.NewGuid(), Name = dto.Name };
        _people.Add(p); return Task.FromResult(p);
    }
    public async Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto, CancellationToken ct)
    {
        var p = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Person not found");
        p.Name = dto.Name; return p;
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var p = await FindAsync(id, ct) ?? throw new KeyNotFoundException("Person not found");
        _people.Remove(p);
    }
}
