using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Pipeline;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IRequestValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken ct, Func<CancellationToken, Task<TResponse>> next)
    {
        var failures = _validators
            .Select(v => v.Validate(request))
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .ToArray();

        if (failures.Length > 0)
        {
            throw new ValidationException(failures);
        }

        return next(ct);
    }
}
