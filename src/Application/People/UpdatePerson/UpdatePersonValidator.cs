using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.UpdatePerson;

public sealed class UpdatePersonValidator : IRequestValidator<UpdatePersonCommand>
{
    public ValidationResult Validate(UpdatePersonCommand request) =>
        string.IsNullOrWhiteSpace(request.Dto.Name)
            ? ValidationResult.Failure("Name must be provided.")
            : ValidationResult.Success;
}
