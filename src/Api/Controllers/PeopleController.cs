using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.People.DeletePerson;
using SolidApiExample.Application.People.GetPerson;
using SolidApiExample.Application.People.ListPeople;

namespace SolidApiExample.Api.Controllers;

[Authorize]
[ApiController, Route("api/people")]
public sealed class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeopleController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDto>> Get(Guid id, CancellationToken ct)
    {
        var person = await _mediator.Send(new GetPersonQuery(id), ct);
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
        var people = await _mediator.Send(new ListPeopleQuery(page, size), ct);
        return Ok(people);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PersonDto>> Create(CreatePersonDto dto, CancellationToken ct)
    {
        var person = await _mediator.Send(new CreatePersonCommand(dto), ct);
        return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PersonDto>> Update(Guid id, UpdatePersonDto dto, CancellationToken ct)
    {
        var person = await _mediator.Send(new UpdatePersonCommand(id, dto), ct);
        return Ok(person);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeletePersonCommand(id), ct);
        return NoContent();
    }
}
