using System;
using System.Collections.Generic;
using System.Linq;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.People;

namespace SolidApiExample.Application.People;

internal static class PeopleMappings
{
    public static PersonDto ToDto(this Person person) =>
        new()
        {
            Id = person.Id,
            Name = person.Name
        };

    public static Paged<PersonDto> ToDto(this Paged<Person> people) =>
        new()
        {
            Items = people.Items.Select(p => p.ToDto()).ToList(),
            Page = people.Page,
            Size = people.Size,
            Total = people.Total
        };

    public static Person ToPerson(this CreatePersonDto dto)
    {
        try
        {
            return Person.Create(dto.Name);
        }
        catch (ArgumentException ex)
        {
            throw new ValidationException(new[] { ex.Message });
        }
    }

    public static string ValidateAndNormalizeName(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException(new[] { "Name must be provided." });
        }

        return name.Trim();
    }
}
