using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.GetOrder;

public sealed class GetOrderValidator : IRequestValidator<Guid>
{
    public ValidationResult Validate(Guid request) =>
        request == Guid.Empty
            ? ValidationResult.Failure("Id must be a non-empty GUID.")
            : ValidationResult.Success;
}
