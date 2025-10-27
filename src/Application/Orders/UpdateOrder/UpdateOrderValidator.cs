using System;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrderValidator : IRequestValidator<UpdateOrderDto>
{
    public ValidationResult Validate(UpdateOrderDto request) =>
        Enum.IsDefined(typeof(OrderStatusDto), request.Status)
            ? ValidationResult.Success
            : ValidationResult.Failure("Status must be provided.");
}
