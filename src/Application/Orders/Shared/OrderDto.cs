using SolidApiExample.Application.Shared;

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
    /// Identifier for the customer associated with the order.
    /// </summary>
    public required Guid CustomerId { get; set; }

    /// <summary>
    /// Current status of the order.
    /// </summary>
    public required OrderStatusDto Status { get; set; }

    /// <summary>
    /// Total amount charged for the order.
    /// </summary>
    public required MoneyDto Total { get; set; }
}
