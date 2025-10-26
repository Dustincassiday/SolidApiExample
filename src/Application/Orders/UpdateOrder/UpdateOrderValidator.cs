using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrderValidator : IRequestValidator<UpdateOrderDto>
{
    public ValidationResult Validate(UpdateOrderDto request) =>
        string.IsNullOrWhiteSpace(request.Status)
            ? ValidationResult.Failure("Status must be provided.")
            : ValidationResult.Success;
}
