using FluentValidation;

namespace SolidApiExample.Application.Customers.ListCustomers;

public sealed class ListCustomersValidator : AbstractValidator<ListCustomersQuery>
{
    public ListCustomersValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page must be zero or greater.");

        RuleFor(request => request.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than zero.");
    }
}
