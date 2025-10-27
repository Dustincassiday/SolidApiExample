using SolidApiExample.Application.Customers;
using SolidApiExample.Application.Customers.CreateCustomer;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.Customers;
using SolidApiExample.Domain.Shared;

namespace SolidApiExample.TestSuite.Application.Customers;

public sealed class CustomersMappingsTests
{
    [Fact]
    public void ToDto_MapsCustomerFields()
    {
        var id = Guid.NewGuid();
        var customer = Customer.FromExisting(id, "Ada Lovelace", Email.Create("ada@example.com"));

        var dto = customer.ToDto();

        Assert.Equal(id, dto.Id);
        Assert.Equal("Ada Lovelace", dto.Name);
        Assert.Equal("ada@example.com", dto.Email);
    }

    [Fact]
    public void ToDto_Paged_MapsCollection()
    {
        var customer = Customer.FromExisting(Guid.NewGuid(), "Ada Lovelace", Email.Create("ada@example.com"));
        var paged = new Paged<Customer>
        {
            Items = new List<Customer> { customer },
            Page = 1,
            Size = 10,
            Total = 30
        };

        var result = paged.ToDto();

        var single = Assert.Single(result.Items);
        Assert.Equal(customer.Id, single.Id);
        Assert.Equal(paged.Page, result.Page);
        Assert.Equal(paged.Size, result.Size);
        Assert.Equal(paged.Total, result.Total);
        Assert.Equal(customer.Email.Value, single.Email);
    }

    [Fact]
    public void ToCustomer_CreatesDomainCustomer()
    {
        var dto = new CreateCustomerDto { Name = "  Ada Lovelace  ", Email = "ada@example.com" };

        var customer = dto.ToCustomer();

        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.Equal("Ada Lovelace", customer.Name);
        Assert.Equal("ada@example.com", customer.Email.Value);
    }

    [Fact]
    public void ToCustomer_ThrowsValidationException_WhenInvalid()
    {
        var dto = new CreateCustomerDto { Name = "", Email = "ada@example.com" };

        var ex = Assert.Throws<ValidationException>(() => dto.ToCustomer());
        Assert.Contains(ex.Errors, error => error.Contains("Name must be provided."));
    }

    [Fact]
    public void ToCustomer_ThrowsValidationException_WhenEmailInvalid()
    {
        var dto = new CreateCustomerDto { Name = "Ada Lovelace", Email = "not-an-email" };

        var ex = Assert.Throws<ValidationException>(() => dto.ToCustomer());
        Assert.Contains(ex.Errors, error => error.Contains("Email is not in a valid format."));
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

    [Fact]
    public void ValidateAndCreateEmail_ReturnsValueObject()
    {
        var email = "ada@example.com".ValidateAndCreateEmail();

        Assert.Equal("ada@example.com", email.Value);
    }

    [Fact]
    public void ValidateAndCreateEmail_Throws_WhenInvalid()
    {
        var ex = Assert.Throws<ValidationException>(() => "invalid".ValidateAndCreateEmail());

        Assert.Contains(ex.Errors, error => error.Contains("Email is not in a valid format."));
    }
}
