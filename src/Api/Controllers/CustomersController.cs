using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.Customers.CreateCustomer;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Application.Customers.UpdateCustomer;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Customers.DeleteCustomer;
using SolidApiExample.Application.Customers.GetCustomer;
using SolidApiExample.Application.Customers.ListCustomers;

namespace SolidApiExample.Api.Controllers;

/// <summary>
/// Provides authenticated endpoints for managing customers records.
/// </summary>
[Authorize]
[ApiController, Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initialises a new instance of the <see cref="CustomersController"/>.
    /// </summary>
    /// <param name="mediator">The mediator used to dispatch requests.</param>
    public CustomersController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Retrieves a single customer by identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="200">Returns the matching customer.</response>
    /// <response code="400">The provided identifier is invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No customer with the supplied identifier exists.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> Get(Guid id, CancellationToken ct)
    {
        var customer = await _mediator.Send(new GetCustomerQuery(id), ct);
        return Ok(customer);
    }

    /// <summary>
    /// Lists customers using zero-based paging.
    /// </summary>
    /// <param name="page">Zero-based page index (defaults to the first page).</param>
    /// <param name="size">Results per page (defaults to 20).</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <remarks>
    /// Increase the <c>page</c> parameter to iterate through the full set while keeping the payloads lightweight.
    /// </remarks>
    /// <response code="200">Returns a paged collection of customers.</response>
    /// <response code="400">One or more paging parameters are invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    [HttpGet]
    public async Task<ActionResult<Paged<CustomerDto>>> List([
        FromQuery] int page = 0,
        [FromQuery] int size = 20,
        CancellationToken ct = default)
    {
        var customers = await _mediator.Send(new ListCustomersQuery(page, size), ct);
        return Ok(customers);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="dto">The customer details.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="201">The customer was created successfully.</response>
    /// <response code="400">The supplied payload failed validation.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto dto, CancellationToken ct)
    {
        var customer = await _mediator.Send(new CreateCustomerCommand(dto), ct);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to update.</param>
    /// <param name="dto">The updated customer details.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="200">Returns the updated customer.</response>
    /// <response code="400">The supplied payload failed validation.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No customer with the supplied identifier exists.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> Update(Guid id, UpdateCustomerDto dto, CancellationToken ct)
    {
        var customer = await _mediator.Send(new UpdateCustomerCommand(id, dto), ct);
        return Ok(customer);
    }

    /// <summary>
    /// Deletes a customer permanently.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="204">The customer was deleted.</response>
    /// <response code="400">The provided identifier was invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No customer with the supplied identifier exists.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCustomerCommand(id), ct);
        return NoContent();
    }
}
