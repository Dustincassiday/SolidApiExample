using FluentValidation;

namespace SolidApiExample.Application.Customers.DeleteCustomer;

public sealed class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage("Id must be a non-empty GUID.");
    }
}
