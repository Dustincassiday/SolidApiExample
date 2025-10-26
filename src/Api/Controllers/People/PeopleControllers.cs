using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.DTOs;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Api.Controllers.People;

[ApiController, Route("api/people")]
public sealed class PeopleController : ControllerBase
{
    private readonly IGetById<Guid, PersonDto> _get;
    private readonly IListItems<PersonDto> _list;
    private readonly ICreate<CreatePersonDto, PersonDto> _create;
    private readonly IUpdate<Guid, UpdatePersonDto, PersonDto> _update;
    private readonly IDelete<Guid> _delete;

    public PeopleController(
        IGetById<Guid, PersonDto> get,
        IListItems<PersonDto> list,
        ICreate<CreatePersonDto, PersonDto> create,
        IUpdate<Guid, UpdatePersonDto, PersonDto> update,
        IDelete<Guid> delete)
    {
        _get = get;
        _list = list;
        _create = create;
        _update = update;
        _delete = delete;
    }

    [HttpGet("{id}")]
    public Task<PersonDto> Get(Guid id, CancellationToken ct) =>
        _get.GetAsync(id, ct);

    [HttpGet]
    public Task<Paged<PersonDto>> List([FromQuery] int page = 0, [FromQuery] int size = 20, CancellationToken ct = default) =>
        _list.ListAsync(page, size, ct);

    [HttpPost]
    public Task<PersonDto> Create(CreatePersonDto dto, CancellationToken ct) =>
        _create.CreateAsync(dto, ct);

    [HttpPut("{id}")]
    public Task<PersonDto> Update(Guid id, UpdatePersonDto dto, CancellationToken ct) =>
        _update.UpdateAsync(id, dto, ct);

    [HttpDelete("{id}")]
    public Task Delete(Guid id, CancellationToken ct) =>
        _delete.DeleteAsync(id, ct);
}
