using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.UpdatePerson;

public sealed class UpdatePersonValidator : IRequestValidator<UpdatePersonDto>
{
    public ValidationResult Validate(UpdatePersonDto request) =>
        string.IsNullOrWhiteSpace(request.Name)
            ? ValidationResult.Failure("Name must be provided.")
            : ValidationResult.Success;
}
