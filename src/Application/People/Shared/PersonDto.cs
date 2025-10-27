namespace SolidApiExample.Application.People.Shared;

/// <summary>
/// Represents a person returned by the API.
/// </summary>
public sealed class PersonDto
{
    /// <summary>
    /// Identifier for the person.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name for the person.
    /// </summary>
    public required string Name { get; set; }
}
