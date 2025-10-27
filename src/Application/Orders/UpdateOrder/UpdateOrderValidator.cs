using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrderValidator : IRequestValidator<UpdateOrderCommand>
{
    public ValidationResult Validate(UpdateOrderCommand request) =>
        Enum.IsDefined(typeof(OrderStatusDto), request.Dto.Status)
            ? ValidationResult.Success
            : ValidationResult.Failure("Status must be provided.");
}
