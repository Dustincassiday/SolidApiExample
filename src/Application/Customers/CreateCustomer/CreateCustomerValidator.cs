using FluentValidation;

namespace SolidApiExample.Application.Customers.CreateCustomer;

public sealed class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(request => request.Dto.Name)
            .NotEmpty()
            .WithMessage("Name must be provided.");
    }
}
