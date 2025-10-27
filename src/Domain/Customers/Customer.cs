namespace SolidApiExample.Domain.Customers;

public sealed class Customer
{
    private Customer(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; private set; }

    public static Customer Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be provided.", nameof(name));
        }

        return new Customer(Guid.NewGuid(), name.Trim());
    }

    public static Customer FromExisting(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Customer id must be provided.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be provided.", nameof(name));
        }

        return new Customer(id, name.Trim());
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be provided.", nameof(name));
        }

        Name = name.Trim();
    }
}
