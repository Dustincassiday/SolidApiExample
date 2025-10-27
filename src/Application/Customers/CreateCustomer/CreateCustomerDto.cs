namespace SolidApiExample.Application.Customers.CreateCustomer;

/// <summary>
/// Represents the payload required to create a customer.
/// </summary>
public sealed class CreateCustomerDto
{
    /// <summary>
    /// The customer's display name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The customer's email address.
    /// </summary>
    public required string Email { get; set; }
}
