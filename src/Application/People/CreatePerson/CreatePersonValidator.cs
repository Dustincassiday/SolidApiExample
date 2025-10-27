using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.CreatePerson;

public sealed class CreatePersonValidator : IRequestValidator<CreatePersonCommand>
{
    public ValidationResult Validate(CreatePersonCommand request) =>
        string.IsNullOrWhiteSpace(request.Dto.Name)
            ? ValidationResult.Failure("Name must be provided.")
            : ValidationResult.Success;
}
