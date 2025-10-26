using System;

namespace SolidApiExample.Application.Orders.Shared;

public sealed class OrderDto
{
    public Guid Id { get; set; }
    public required Guid PersonId { get; set; }
    public required string Status { get; set; }
}
