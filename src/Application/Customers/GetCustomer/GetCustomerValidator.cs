using FluentValidation;

namespace SolidApiExample.Application.Customers.GetCustomer;

public sealed class GetCustomerValidator : AbstractValidator<GetCustomerQuery>
{
    public GetCustomerValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage("Id must be a non-empty GUID.");
    }
}
