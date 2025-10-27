using FluentValidation;

namespace SolidApiExample.Application.Orders.CreateOrder;

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(request => request.Dto.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId must be a non-empty GUID.");

        RuleFor(request => request.Dto.Total)
            .NotNull()
            .WithMessage("Total must be provided.");

        RuleFor(request => request.Dto.Total.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative.")
            .When(request => request.Dto.Total is not null);

        RuleFor(request => request.Dto.Total.Currency)
            .NotEmpty()
            .WithMessage("Total currency must be provided.")
            .Length(3)
            .WithMessage("Total currency must be a three-letter code.")
            .When(request => request.Dto.Total is not null);
    }
}
