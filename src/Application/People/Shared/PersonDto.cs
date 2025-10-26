using System;

namespace SolidApiExample.Application.People.Shared;

public sealed class PersonDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
