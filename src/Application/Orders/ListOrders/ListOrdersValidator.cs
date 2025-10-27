using FluentValidation;

namespace SolidApiExample.Application.Orders.ListOrders;

public sealed class ListOrdersValidator : AbstractValidator<ListOrdersQuery>
{
    public ListOrdersValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page must be zero or greater.");

        RuleFor(request => request.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than zero.");
    }
}
