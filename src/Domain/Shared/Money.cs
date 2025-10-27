using System.Globalization;
using System.Linq;

namespace SolidApiExample.Domain.Shared;

/// <summary>
/// Represents an amount of money in a specific currency.
/// </summary>
public sealed record Money
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Monetary amount rounded to two decimal places.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// ISO currency code.
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Creates a new instance after validation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the amount is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when the currency is invalid.</exception>
    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency must be provided.", nameof(currency));
        }

        var normalizedCurrency = currency.Trim().ToUpperInvariant();

        if (normalizedCurrency.Length is not 3 || !normalizedCurrency.All(char.IsLetter))
        {
            throw new ArgumentException("Currency must be a three-letter code.", nameof(currency));
        }

        var roundedAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

        return new Money(roundedAmount, normalizedCurrency);
    }

    public bool HasSameCurrency(Money other) =>
        Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);

        var result = Amount - other.Amount;
        if (result < 0)
        {
            throw new InvalidOperationException("Resulting amount cannot be negative.");
        }

        return new Money(result, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (!HasSameCurrency(other))
        {
            throw new InvalidOperationException("Money values must share the same currency.");
        }
    }

    public override string ToString() => string.Create(
        CultureInfo.InvariantCulture,
        $"{Currency} {Amount:0.00}");
}
