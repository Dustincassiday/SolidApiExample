namespace SolidApiExample.Domain.Orders;

public sealed class Order
{
    private Order(Guid id, Guid personId, OrderStatus status)
    {
        Id = id;
        PersonId = personId;
        Status = status;
    }

    public Guid Id { get; }
    public Guid PersonId { get; }
    public OrderStatus Status { get; private set; }

    public static Order Create(Guid personId, OrderStatus status)
    {
        if (personId == Guid.Empty)
        {
            throw new ArgumentException("Person id must be provided.", nameof(personId));
        }

        return new Order(Guid.NewGuid(), personId, status);
    }

    public static Order FromExisting(Guid id, Guid personId, OrderStatus status)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Order id must be provided.", nameof(id));
        }

        if (personId == Guid.Empty)
        {
            throw new ArgumentException("Person id must be provided.", nameof(personId));
        }

        return new Order(id, personId, status);
    }

    public void UpdateStatus(OrderStatus status) => Status = status;
}
