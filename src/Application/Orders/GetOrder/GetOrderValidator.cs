using FluentValidation;

namespace SolidApiExample.Application.Orders.GetOrder;

public sealed class GetOrderValidator : AbstractValidator<GetOrderQuery>
{
    public GetOrderValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage("Id must be a non-empty GUID.");
    }
}
