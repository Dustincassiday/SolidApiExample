using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Api.Controllers;

/// <summary>
/// Exposes CRUD-style endpoints for working with orders.
/// </summary>
[Authorize]
[ApiController, Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initialises a new instance of the <see cref="OrdersController"/>.
    /// </summary>
    /// <param name="mediator">The mediator used to dispatch requests.</param>
    public OrdersController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Retrieves an order by identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="200">Returns the matching order.</response>
    /// <response code="400">The provided identifier is invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No order with the supplied identifier exists.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> Get(Guid id, CancellationToken ct)
    {
        var order = await _mediator.Send(new GetOrderQuery(id), ct);
        return Ok(order);
    }

    /// <summary>
    /// Lists orders using zero-based paging.
    /// </summary>
    /// <param name="page">Zero-based page index (defaults to the first page).</param>
    /// <param name="size">Results per page (defaults to 20).</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <remarks>
    /// Combine <c>page</c> and <c>size</c> to page through orders while keeping responses predictable.
    /// </remarks>
    /// <response code="200">Returns a paged collection of orders.</response>
    /// <response code="400">One or more paging parameters are invalid.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    [HttpGet]
    public async Task<ActionResult<Paged<OrderDto>>> List([
        FromQuery] int page = 0,
        [FromQuery] int size = 20,
        CancellationToken ct = default)
    {
        var orders = await _mediator.Send(new ListOrdersQuery(page, size), ct);
        return Ok(orders);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="dto">The order details.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="201">The order was created successfully.</response>
    /// <response code="400">The supplied payload failed validation.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto, CancellationToken ct)
    {
        var order = await _mediator.Send(new CreateOrderCommand(dto), ct);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order to update.</param>
    /// <param name="dto">The new order details.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <response code="200">Returns the updated order.</response>
    /// <response code="400">The supplied payload failed validation.</response>
    /// <response code="401">The request lacks valid authentication credentials.</response>
    /// <response code="404">No order with the supplied identifier exists.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<OrderDto>> Update(Guid id, UpdateOrderDto dto, CancellationToken ct)
    {
        var order = await _mediator.Send(new UpdateOrderCommand(id, dto), ct);
        return Ok(order);
    }
}
