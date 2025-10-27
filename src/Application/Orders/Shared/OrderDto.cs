namespace SolidApiExample.Application.Orders.Shared;

/// <summary>
/// Represents an order returned by the API.
/// </summary>
public sealed class OrderDto
{
    /// <summary>
    /// Identifier for the order.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifier for the person associated with the order.
    /// </summary>
    public required Guid PersonId { get; set; }

    /// <summary>
    /// Current status of the order.
    /// </summary>
    public required OrderStatusDto Status { get; set; }
}
