using System;
using System.Linq;
using SolidApiExample.Application.Customers.CreateCustomer;
using SolidApiExample.Application.Customers.DeleteCustomer;
using SolidApiExample.Application.Customers.GetCustomer;
using SolidApiExample.Application.Customers.ListCustomers;
using SolidApiExample.Application.Customers.UpdateCustomer;
using Xunit;

namespace SolidApiExample.TestSuite.Application.Customers;

public sealed class CustomersValidatorsTests
{
    [Fact]
    public void CreateCustomerValidator_ReturnsFailure_WhenNameMissing()
    {
        var validator = new CreateCustomerValidator();
        var dto = new CreateCustomerDto { Name = " " };

        var result = validator.Validate(new CreateCustomerCommand(dto));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Name must be provided.");
    }

    [Fact]
    public void CreateCustomerValidator_ReturnsSuccess_WhenNameProvided()
    {
        var validator = new CreateCustomerValidator();
        var dto = new CreateCustomerDto { Name = "Ada Lovelace" };

        var result = validator.Validate(new CreateCustomerCommand(dto));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void UpdateCustomerValidator_ReturnsFailure_WhenNameMissing()
    {
        var validator = new UpdateCustomerValidator();
        var dto = new UpdateCustomerDto { Name = "" };

        var result = validator.Validate(new UpdateCustomerCommand(Guid.NewGuid(), dto));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Name must be provided.");
    }

    [Fact]
    public void UpdateCustomerValidator_ReturnsSuccess_WhenNameProvided()
    {
        var validator = new UpdateCustomerValidator();
        var dto = new UpdateCustomerDto { Name = "Grace Hopper" };

        var result = validator.Validate(new UpdateCustomerCommand(Guid.NewGuid(), dto));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetCustomerValidator_ReturnsFailure_WhenIdEmpty()
    {
        var validator = new GetCustomerValidator();

        var result = validator.Validate(new GetCustomerQuery(Guid.Empty));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Id must be a non-empty GUID.");
    }

    [Fact]
    public void GetCustomerValidator_ReturnsSuccess_WhenIdProvided()
    {
        var validator = new GetCustomerValidator();

        var result = validator.Validate(new GetCustomerQuery(Guid.NewGuid()));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void DeleteCustomerValidator_ReturnsFailure_WhenIdEmpty()
    {
        var validator = new DeleteCustomerValidator();

        var result = validator.Validate(new DeleteCustomerCommand(Guid.Empty));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Id must be a non-empty GUID.");
    }

    [Fact]
    public void DeleteCustomerValidator_ReturnsSuccess_WhenIdProvided()
    {
        var validator = new DeleteCustomerValidator();

        var result = validator.Validate(new DeleteCustomerCommand(Guid.NewGuid()));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ListCustomersValidator_ReturnsFailure_WhenPageNegative()
    {
        var validator = new ListCustomersValidator();

        var result = validator.Validate(new ListCustomersQuery(-1, 5));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Page must be zero or greater.");
    }

    [Fact]
    public void ListCustomersValidator_ReturnsFailure_WhenSizeNotPositive()
    {
        var validator = new ListCustomersValidator();

        var result = validator.Validate(new ListCustomersQuery(0, 0));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Size must be greater than zero.");
    }

    [Fact]
    public void ListCustomersValidator_ReturnsSuccess_ForValidArguments()
    {
        var validator = new ListCustomersValidator();

        var result = validator.Validate(new ListCustomersQuery(2, 25));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
