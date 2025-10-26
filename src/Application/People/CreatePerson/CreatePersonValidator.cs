using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.CreatePerson;

public sealed class CreatePersonValidator : IRequestValidator<CreatePersonDto>
{
    public ValidationResult Validate(CreatePersonDto request) =>
        string.IsNullOrWhiteSpace(request.Name)
            ? ValidationResult.Failure("Name must be provided.")
            : ValidationResult.Success;
}
