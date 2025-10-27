using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.ListPeople;

public sealed class ListPeopleValidator : IRequestValidator<ListPeopleQuery>
{
    public ValidationResult Validate(ListPeopleQuery request)
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
