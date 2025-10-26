using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.ListPeople;

public sealed class ListPeopleValidator : IRequestValidator<(int page, int size)>
{
    public ValidationResult Validate((int page, int size) request)
    {
        var (page, size) = request;
        if (page < 0)
        {
            return ValidationResult.Failure("Page must be zero or greater.");
        }

        if (size <= 0)
        {
            return ValidationResult.Failure("Size must be greater than zero.");
        }

        return ValidationResult.Success;
    }
}
