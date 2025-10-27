using FluentValidation;

namespace SolidApiExample.Application.Orders.UpdateOrder;

public sealed class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(request => request.Dto.Status)
            .IsInEnum()
            .WithMessage("Status must be provided.");
    }
}
