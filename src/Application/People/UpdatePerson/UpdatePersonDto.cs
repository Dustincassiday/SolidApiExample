namespace SolidApiExample.Application.People.UpdatePerson;

/// <summary>
/// Represents the payload required to update a person's details.
/// </summary>
public sealed class UpdatePersonDto
{
    /// <summary>
    /// The person's updated name.
    /// </summary>
    public required string Name { get; set; }
}
