using SolidApiExample.Application.People;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.People;

namespace SolidApiExample.TestSuite.Application.People;

public sealed class PeopleMappingsTests
{
    [Fact]
    public void ToDto_MapsPersonFields()
    {
        var id = Guid.NewGuid();
        var person = Person.FromExisting(id, "Ada Lovelace");

        var dto = person.ToDto();

        Assert.Equal(id, dto.Id);
        Assert.Equal("Ada Lovelace", dto.Name);
    }

    [Fact]
    public void ToDto_Paged_MapsCollection()
    {
        var person = Person.FromExisting(Guid.NewGuid(), "Ada Lovelace");
        var paged = new Paged<Person>
        {
            Items = new List<Person> { person },
            Page = 1,
            Size = 10,
            Total = 30
        };

        var result = paged.ToDto();

        var single = Assert.Single(result.Items);
        Assert.Equal(person.Id, single.Id);
        Assert.Equal(paged.Page, result.Page);
        Assert.Equal(paged.Size, result.Size);
        Assert.Equal(paged.Total, result.Total);
    }

    [Fact]
    public void ToPerson_CreatesDomainPerson()
    {
        var dto = new CreatePersonDto { Name = "  Ada Lovelace  " };

        var person = dto.ToPerson();

        Assert.NotEqual(Guid.Empty, person.Id);
        Assert.Equal("Ada Lovelace", person.Name);
    }

    [Fact]
    public void ToPerson_ThrowsValidationException_WhenInvalid()
    {
        var dto = new CreatePersonDto { Name = "" };

        var ex = Assert.Throws<ValidationException>(() => dto.ToPerson());
        Assert.Contains(ex.Errors, error => error.Contains("Name must be provided."));
    }

    [Fact]
    public void ValidateAndNormalizeName_TrimsAndReturnsValue()
    {
        var value = "  Alan Turing  ";

        var normalized = value.ValidateAndNormalizeName();

        Assert.Equal("Alan Turing", normalized);
    }

    [Fact]
    public void ValidateAndNormalizeName_Throws_WhenBlank()
    {
        var ex = Assert.Throws<ValidationException>(() => "".ValidateAndNormalizeName());
        Assert.Contains("Name must be provided.", ex.Errors);
    }
}
