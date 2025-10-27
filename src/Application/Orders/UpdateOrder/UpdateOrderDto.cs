using SolidApiExample.Application.Orders;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrderDto
{
    public required OrderStatusDto Status { get; set; }
}
