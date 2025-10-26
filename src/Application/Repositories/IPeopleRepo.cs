using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.Repositories;

public interface IPeopleRepo
{
    Task<PersonDto?> FindAsync(Guid id, CancellationToken ct);
    Task<Paged<PersonDto>> ListAsync(int page, int size, CancellationToken ct);
    Task<PersonDto> AddAsync(CreatePersonDto dto, CancellationToken ct);
    Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
