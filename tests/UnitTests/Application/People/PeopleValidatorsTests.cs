using System;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.DeletePerson;
using SolidApiExample.Application.People.GetPerson;
using SolidApiExample.Application.People.ListPeople;
using SolidApiExample.Application.People.UpdatePerson;
using Xunit;

namespace SolidApiExample.UnitTests.Application.People;

public sealed class PeopleValidatorsTests
{
    [Fact]
    public void CreatePersonValidator_ReturnsFailure_WhenNameMissing()
    {
        var validator = new CreatePersonValidator();
        var dto = new CreatePersonDto { Name = " " };

        var result = validator.Validate(new CreatePersonCommand(dto));

        Assert.False(result.IsValid);
        Assert.Contains("Name must be provided.", result.Errors);
    }

    [Fact]
    public void CreatePersonValidator_ReturnsSuccess_WhenNameProvided()
    {
        var validator = new CreatePersonValidator();
        var dto = new CreatePersonDto { Name = "Ada Lovelace" };

        var result = validator.Validate(new CreatePersonCommand(dto));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void UpdatePersonValidator_ReturnsFailure_WhenNameMissing()
    {
        var validator = new UpdatePersonValidator();
        var dto = new UpdatePersonDto { Name = "" };

        var result = validator.Validate(new UpdatePersonCommand(Guid.NewGuid(), dto));

        Assert.False(result.IsValid);
        Assert.Contains("Name must be provided.", result.Errors);
    }

    [Fact]
    public void UpdatePersonValidator_ReturnsSuccess_WhenNameProvided()
    {
        var validator = new UpdatePersonValidator();
        var dto = new UpdatePersonDto { Name = "Grace Hopper" };

        var result = validator.Validate(new UpdatePersonCommand(Guid.NewGuid(), dto));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetPersonValidator_ReturnsFailure_WhenIdEmpty()
    {
        var validator = new GetPersonValidator();

        var result = validator.Validate(new GetPersonQuery(Guid.Empty));

        Assert.False(result.IsValid);
        Assert.Contains("Id must be a non-empty GUID.", result.Errors);
    }

    [Fact]
    public void GetPersonValidator_ReturnsSuccess_WhenIdProvided()
    {
        var validator = new GetPersonValidator();

        var result = validator.Validate(new GetPersonQuery(Guid.NewGuid()));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void DeletePersonValidator_ReturnsFailure_WhenIdEmpty()
    {
        var validator = new DeletePersonValidator();

        var result = validator.Validate(new DeletePersonCommand(Guid.Empty));

        Assert.False(result.IsValid);
        Assert.Contains("Id must be a non-empty GUID.", result.Errors);
    }

    [Fact]
    public void DeletePersonValidator_ReturnsSuccess_WhenIdProvided()
    {
        var validator = new DeletePersonValidator();

        var result = validator.Validate(new DeletePersonCommand(Guid.NewGuid()));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ListPeopleValidator_ReturnsFailure_WhenPageNegative()
    {
        var validator = new ListPeopleValidator();

        var result = validator.Validate(new ListPeopleQuery(-1, 5));

        Assert.False(result.IsValid);
        Assert.Contains("Page must be zero or greater.", result.Errors);
    }

    [Fact]
    public void ListPeopleValidator_ReturnsFailure_WhenSizeNotPositive()
    {
        var validator = new ListPeopleValidator();

        var result = validator.Validate(new ListPeopleQuery(0, 0));

        Assert.False(result.IsValid);
        Assert.Contains("Size must be greater than zero.", result.Errors);
    }

    [Fact]
    public void ListPeopleValidator_ReturnsSuccess_ForValidArguments()
    {
        var validator = new ListPeopleValidator();

        var result = validator.Validate(new ListPeopleQuery(2, 25));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
