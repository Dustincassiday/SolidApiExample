using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Api.Controllers;

[Authorize]
[ApiController, Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IGetById<Guid, OrderDto> _get;
    private readonly IListItems<OrderDto> _list;
    private readonly ICreate<CreateOrderDto, OrderDto> _create;
    private readonly IUpdate<Guid, UpdateOrderDto, OrderDto> _update;

    public OrdersController(
        IGetById<Guid, OrderDto> get,
        IListItems<OrderDto> list,
        ICreate<CreateOrderDto, OrderDto> create,
        IUpdate<Guid, UpdateOrderDto, OrderDto> update)
    {
        _get = get;
        _list = list;
        _create = create;
        _update = update;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OrderDto>> Get(Guid id, CancellationToken ct)
    {
        var order = await _get.GetAsync(id, ct);
        return Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Paged<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Paged<OrderDto>>> List([
        FromQuery] int page = 0,
        [FromQuery] int size = 20,
        CancellationToken ct = default)
    {
        var orders = await _list.ListAsync(page, size, ct);
        return Ok(orders);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto, CancellationToken ct)
    {
        var order = await _create.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OrderDto>> Update(Guid id, UpdateOrderDto dto, CancellationToken ct)
    {
        var order = await _update.UpdateAsync(id, dto, ct);
        return Ok(order);
    }
}
