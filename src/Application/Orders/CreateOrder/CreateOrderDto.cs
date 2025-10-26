using System;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrderDto
{
    public required Guid PersonId { get; set; }
    public required string Status { get; set; }
}
