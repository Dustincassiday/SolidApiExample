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

/// <summary>
/// Provides authenticated endpoints for managing people records.
/// </summary>
[Authorize]
[ApiController, Route("api/people")]
public sealed class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initialises a new instance of the <see cref="PeopleController"/>.
    /// </summary>
    /// <param name="mediator">The mediator used to dispatch requests.</param>
    public PeopleController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Retrieves a single person by identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the person.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="200">Returns the matching person.</response>
    /// <response code="400">The provided identifier is invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No person with the supplied identifier exists.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> Get(Guid id, CancellationToken ct)
    {
        var person = await _mediator.Send(new GetPersonQuery(id), ct);
        return Ok(person);
    }

    /// <summary>
    /// Lists people using zero-based paging.
    /// </summary>
    /// <param name="page">Zero-based page index (defaults to the first page).</param>
    /// <param name="size">Results per page (defaults to 20).</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <remarks>
    /// Increase the <c>page</c> parameter to iterate through the full set while keeping the payloads lightweight.
    /// </remarks>
    /// <response code="200">Returns a paged collection of people.</response>
    /// <response code="400">One or more paging parameters are invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    [HttpGet]
    public async Task<ActionResult<Paged<PersonDto>>> List([
        FromQuery] int page = 0,
        [FromQuery] int size = 20,
        CancellationToken ct = default)
    {
        var people = await _mediator.Send(new ListPeopleQuery(page, size), ct);
        return Ok(people);
    }

    /// <summary>
    /// Creates a new person.
    /// </summary>
    /// <param name="dto">The person details.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="201">The person was created successfully.</response>
    /// <response code="400">The supplied payload failed validation.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create(CreatePersonDto dto, CancellationToken ct)
    {
        var person = await _mediator.Send(new CreatePersonCommand(dto), ct);
        return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
    }

    /// <summary>
    /// Updates an existing person.
    /// </summary>
    /// <param name="id">The unique identifier of the person to update.</param>
    /// <param name="dto">The updated person details.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="200">Returns the updated person.</response>
    /// <response code="400">The supplied payload failed validation.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No person with the supplied identifier exists.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonDto>> Update(Guid id, UpdatePersonDto dto, CancellationToken ct)
    {
        var person = await _mediator.Send(new UpdatePersonCommand(id, dto), ct);
        return Ok(person);
    }

    /// <summary>
    /// Deletes a person permanently.
    /// </summary>
    /// <param name="id">The unique identifier of the person to delete.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="204">The person was deleted.</response>
    /// <response code="400">The provided identifier was invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No person with the supplied identifier exists.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeletePersonCommand(id), ct);
        return NoContent();
    }
}
