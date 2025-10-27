namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrderDto
{
    public required Guid PersonId { get; set; }
    public required OrderStatusDto Status { get; set; }
}
