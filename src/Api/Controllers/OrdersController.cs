using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Api.Controllers;

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
    public Task<OrderDto> Get(Guid id, CancellationToken ct) =>
        _get.GetAsync(id, ct);

    [HttpGet]
    public Task<Paged<OrderDto>> List([FromQuery] int page = 0, [FromQuery] int size = 20, CancellationToken ct = default) =>
        _list.ListAsync(page, size, ct);

    [HttpPost]
    public Task<OrderDto> Create(CreateOrderDto dto, CancellationToken ct) =>
        _create.CreateAsync(dto, ct);

    [HttpPut("{id}")]
    public Task<OrderDto> Update(Guid id, UpdateOrderDto dto, CancellationToken ct) =>
        _update.UpdateAsync(id, dto, ct);
}
