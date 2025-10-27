namespace SolidApiExample.Application.People.CreatePerson;

/// <summary>
/// Represents the payload required to create a person.
/// </summary>
public sealed class CreatePersonDto
{
    /// <summary>
    /// The person's display name.
    /// </summary>
    public required string Name { get; set; }
}
