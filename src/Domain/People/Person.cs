namespace SolidApiExample.Domain.People;

public sealed class Person
{
    private Person(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; private set; }

    public static Person Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be provided.", nameof(name));
        }

        return new Person(Guid.NewGuid(), name.Trim());
    }

    public static Person FromExisting(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Person id must be provided.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be provided.", nameof(name));
        }

        return new Person(id, name.Trim());
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
