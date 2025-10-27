using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.GetPerson;

public sealed class GetPersonValidator : IRequestValidator<GetPersonQuery>
{
    public ValidationResult Validate(GetPersonQuery request) =>
        request.Id == Guid.Empty
            ? ValidationResult.Failure("Id must be a non-empty GUID.")
            : ValidationResult.Success;
}
