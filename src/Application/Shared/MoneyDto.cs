namespace SolidApiExample.Application.Shared;

/// <summary>
/// Represents a money value in API payloads.
/// </summary>
public sealed class MoneyDto
{
    /// <summary>
    /// The numeric amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Three-letter ISO currency code.
    /// </summary>
    public required string Currency { get; set; }
}
