namespace SolidApiExample.Application.Customers.Shared;

/// <summary>
/// Represents a customer returned by the API.
/// </summary>
public sealed class CustomerDto
{
    /// <summary>
    /// Identifier for the customer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name for the customer.
    /// </summary>
    public required string Name { get; set; }
}
