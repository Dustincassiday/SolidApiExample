namespace SolidApiExample.Application.Orders.CreateOrder;

/// <summary>
/// Represents the payload required to create an order.
/// </summary>
public sealed class CreateOrderDto
{
    /// <summary>
    /// The identifier of the customer who owns the order.
    /// </summary>
    public required Guid CustomerId { get; set; }

    /// <summary>
    /// The starting status for the order.
    /// </summary>
    public required OrderStatusDto Status { get; set; }
}
