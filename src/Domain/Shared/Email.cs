namespace SolidApiExample.Domain.Shared;

/// <summary>
/// Represents a validated email address.
/// </summary>
public sealed record Email
{
    private Email(string value) => Value = value;

    /// <summary>
    /// Underlying email value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Email"/> after validation.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the value is not a valid email address.</exception>
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email must be provided.", nameof(value));
        }

        var trimmed = value.Trim();

        if (!IsValid(trimmed))
        {
            throw new ArgumentException("Email is not in a valid format.", nameof(value));
        }

        return new Email(trimmed);
    }

    private static bool IsValid(string value)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(value);
            return addr.Address == value;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => Value;
}
