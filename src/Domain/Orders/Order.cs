namespace SolidApiExample.Domain.Orders;

public sealed class Order
{
    private Order(Guid id, Guid customerId, OrderStatus status)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
    }

    public Guid Id { get; }
    public Guid CustomerId { get; }
    public OrderStatus Status { get; private set; }

    public static Order Create(Guid customerId, OrderStatus status)
    {
        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer id must be provided.", nameof(customerId));
        }

        return new Order(Guid.NewGuid(), customerId, status);
    }

    public static Order FromExisting(Guid id, Guid customerId, OrderStatus status)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Order id must be provided.", nameof(id));
        }

        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer id must be provided.", nameof(customerId));
        }

        return new Order(id, customerId, status);
    }

    public void UpdateStatus(OrderStatus status) => Status = status;
}
