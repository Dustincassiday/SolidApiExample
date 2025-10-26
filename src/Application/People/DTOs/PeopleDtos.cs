namespace SolidApiExample.Application.People.DTOs;

public sealed class PersonDto { public System.Guid Id { get; set; } public required string Name { get; set; } }
public sealed class CreatePersonDto { public required string Name { get; set; } }
public sealed class UpdatePersonDto { public required string Name { get; set; } }
