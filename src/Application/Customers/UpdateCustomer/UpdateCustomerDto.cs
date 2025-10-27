namespace SolidApiExample.Application.Customers.UpdateCustomer;

/// <summary>
/// Represents the payload required to update a customer's details.
/// </summary>
public sealed class UpdateCustomerDto
{
    /// <summary>
    /// The customer's updated name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The customer's updated email address.
    /// </summary>
    public required string Email { get; set; }
}
