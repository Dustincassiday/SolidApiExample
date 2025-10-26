namespace SolidApiExample.Application.Validation;

public interface IRequestValidator<in TRequest>
{
    ValidationResult Validate(TRequest request);
}

public sealed record ValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    public static ValidationResult Success { get; } = new(true, Array.Empty<string>());

    public static ValidationResult Failure(params string[] errors) =>
        new(false, errors);
}
