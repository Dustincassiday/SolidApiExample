using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrderValidator : IRequestValidator<CreateOrderDto>
{
    public ValidationResult Validate(CreateOrderDto request)
    {
        if (request.PersonId == Guid.Empty)
        {
            return ValidationResult.Failure("PersonId must be a non-empty GUID.");
        }

        if (!Enum.IsDefined(typeof(OrderStatusDto), request.Status))
        {
            return ValidationResult.Failure("Status must be provided.");
        }

        return ValidationResult.Success;
    }
}
