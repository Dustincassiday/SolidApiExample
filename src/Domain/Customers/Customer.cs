using SolidApiExample.Domain.Shared;

namespace SolidApiExample.Domain.Customers;

public sealed class Customer
{
    private Customer(Guid id, string name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public Guid Id { get; }
    public string Name { get; private set; }
    public Email Email { get; private set; }

    public static Customer Create(string name, Email email)
    {
        var normalizedName = NormalizeName(name);
        var verifiedEmail = email ?? throw new ArgumentNullException(nameof(email));
        return new Customer(Guid.NewGuid(), normalizedName, verifiedEmail);
    }

    public static Customer FromExisting(Guid id, string name, Email email)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Customer id must be provided.", nameof(id));
        }

        var normalizedName = NormalizeName(name);
        var verifiedEmail = email ?? throw new ArgumentNullException(nameof(email));

        return new Customer(id, normalizedName, verifiedEmail);
    }

    public void Rename(string name)
    {
        Name = NormalizeName(name);
    }

    public void UpdateEmail(Email email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be provided.", nameof(name));
        }

        return name.Trim();
    }
}
