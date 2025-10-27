using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrderValidator : IRequestValidator<CreateOrderCommand>
{
    public ValidationResult Validate(CreateOrderCommand request)
    {
        if (request.Dto.PersonId == Guid.Empty)
        {
            return ValidationResult.Failure("PersonId must be a non-empty GUID.");
        }

        if (!Enum.IsDefined(typeof(OrderStatusDto), request.Dto.Status))
        {
            return ValidationResult.Failure("Status must be provided.");
        }

        return ValidationResult.Success;
    }
}
