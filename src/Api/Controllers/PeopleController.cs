using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Api.Controllers;

[Authorize]
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
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDto>> Get(Guid id, CancellationToken ct)
    {
        var person = await _get.GetAsync(id, ct);
        return Ok(person);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Paged<PersonDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Paged<PersonDto>>> List([
        FromQuery] int page = 0,
        [FromQuery] int size = 20,
        CancellationToken ct = default)
    {
        var people = await _list.ListAsync(page, size, ct);
        return Ok(people);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PersonDto>> Create(CreatePersonDto dto, CancellationToken ct)
    {
        var person = await _create.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PersonDto>> Update(Guid id, UpdatePersonDto dto, CancellationToken ct)
    {
        var person = await _update.UpdateAsync(id, dto, ct);
        return Ok(person);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _delete.DeleteAsync(id, ct);
        return NoContent();
    }
}
