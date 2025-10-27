using SolidApiExample.Application.Customers.CreateCustomer;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.Customers;

namespace SolidApiExample.Application.Customers;

internal static class CustomersMappings
{
    public static CustomerDto ToDto(this Customer customer) =>
        new()
        {
            Id = customer.Id,
            Name = customer.Name
        };

    public static Paged<CustomerDto> ToDto(this Paged<Customer> customers) =>
        new()
        {
            Items = customers.Items.Select(p => p.ToDto()).ToList(),
            Page = customers.Page,
            Size = customers.Size,
            Total = customers.Total
        };

    public static Customer ToCustomer(this CreateCustomerDto dto)
    {
        try
        {
            return Customer.Create(dto.Name);
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
