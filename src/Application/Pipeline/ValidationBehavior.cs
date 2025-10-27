using FluentValidation;
using MediatR;
using AppValidationException = SolidApiExample.Application.Validation.ValidationException;

namespace SolidApiExample.Application.Pipeline;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IValidator<TRequest>[] _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators as IValidator<TRequest>[] ?? validators.ToArray();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Length == 0)
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null && !string.IsNullOrWhiteSpace(failure.ErrorMessage))
            .Select(failure => failure!.ErrorMessage)
            .ToArray();

        if (failures.Length > 0)
        {
            throw new AppValidationException(failures);
        }

        return await next();
    }
}
