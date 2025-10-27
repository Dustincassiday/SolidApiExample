using SolidApiExample.Application.Orders;

namespace SolidApiExample.Application.Orders.UpdateOrder;

/// <summary>
/// Represents the payload required to update an order.
/// </summary>
public sealed class UpdateOrderDto
{
    /// <summary>
    /// The new status for the order.
    /// </summary>
    public required OrderStatusDto Status { get; set; }
}
