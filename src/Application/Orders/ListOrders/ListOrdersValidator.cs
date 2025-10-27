using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.Orders.ListOrders;

public sealed class ListOrdersValidator : IRequestValidator<ListOrdersQuery>
{
    public ValidationResult Validate(ListOrdersQuery request)
    {
        if (request.Page < 0)
        {
            return ValidationResult.Failure("Page must be zero or greater.");
        }

        if (request.Size <= 0)
        {
            return ValidationResult.Failure("Size must be greater than zero.");
        }

        return ValidationResult.Success;
    }
}
