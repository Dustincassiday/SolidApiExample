using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.GetOrder;

public sealed class GetOrderValidator : IRequestValidator<GetOrderQuery>
{
    public ValidationResult Validate(GetOrderQuery request) =>
        request.Id == Guid.Empty
            ? ValidationResult.Failure("Id must be a non-empty GUID.")
            : ValidationResult.Success;
}
