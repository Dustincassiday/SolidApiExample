using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.DeletePerson;

public sealed class DeletePersonValidator : IRequestValidator<DeletePersonCommand>
{
    public ValidationResult Validate(DeletePersonCommand request) =>
        request.Id == Guid.Empty
            ? ValidationResult.Failure("Id must be a non-empty GUID.")
            : ValidationResult.Success;
}
