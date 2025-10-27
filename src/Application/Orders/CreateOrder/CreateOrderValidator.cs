using FluentValidation;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(request => request.Dto.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId must be a non-empty GUID.");

        RuleFor(request => request.Dto.Status)
            .IsInEnum()
            .WithMessage("Status must be provided.");
    }
}
