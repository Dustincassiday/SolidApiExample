namespace SolidApiExample.Application.Orders.DTOs;

public sealed class OrderDto { public System.Guid Id { get; set; } public required System.Guid PersonId { get; set; } public required string Status { get; set; } }
public sealed class CreateOrderDto { public required System.Guid PersonId { get; set; } public required string Status { get; set; } }
public sealed class UpdateOrderDto { public required string Status { get; set; } }
