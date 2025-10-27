using FluentValidation;

namespace SolidApiExample.Application.Customers.UpdateCustomer;

public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(request => request.Dto.Name)
            .NotEmpty()
            .WithMessage("Name must be provided.");

        RuleFor(request => request.Dto.Email)
            .NotEmpty()
            .WithMessage("Email must be provided.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");
    }
}
