using SolidApiExample.Domain.Shared;

namespace SolidApiExample.Domain.Orders;

public sealed class Order
{
    private Order(Guid id, Guid customerId, OrderStatus status, Money total)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
        Total = total;
    }

    public Guid Id { get; }
    public Guid CustomerId { get; }
    public OrderStatus Status { get; private set; }
    public Money Total { get; private set; }

    public static Order Create(Guid customerId, Money total)
    {
        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer id must be provided.", nameof(customerId));
        }

        var verifiedTotal = total ?? throw new ArgumentNullException(nameof(total));

        return new Order(Guid.NewGuid(), customerId, OrderStatus.New, verifiedTotal);
    }

    public static Order FromExisting(Guid id, Guid customerId, OrderStatus status, Money total)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Order id must be provided.", nameof(id));
        }

        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer id must be provided.", nameof(customerId));
        }

        var verifiedTotal = total ?? throw new ArgumentNullException(nameof(total));

        return new Order(id, customerId, status, verifiedTotal);
    }

    public void UpdateStatus(OrderStatus status)
    {
        if (!IsValidTransition(Status, status))
        {
            throw new InvalidOperationException($"Cannot transition order from {Status} to {status}.");
        }

        Status = status;
    }

    private static bool IsValidTransition(OrderStatus current, OrderStatus next) =>
        next switch
        {
            OrderStatus.New => current == OrderStatus.New,
            OrderStatus.Paid => current is OrderStatus.New or OrderStatus.Paid,
            OrderStatus.Shipped => current is OrderStatus.Paid or OrderStatus.Shipped,
            _ => false
        };
}
