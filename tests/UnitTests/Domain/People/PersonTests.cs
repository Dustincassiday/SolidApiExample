using System;
using SolidApiExample.Domain.People;
using Xunit;

namespace SolidApiExample.UnitTests.Domain.People;

public sealed class PersonTests
{
    [Fact]
    public void Create_WithEmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => Person.Create(" "));
    }

    [Fact]
    public void FromExisting_WithEmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(() => Person.FromExisting(Guid.Empty, "Ada"));
    }

    [Fact]
    public void Rename_UpdatesName()
    {
        var person = Person.Create("Ada");

        person.Rename("Ada King");

        Assert.Equal("Ada King", person.Name);
    }
}
