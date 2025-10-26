using System;
using SolidApiExample.Application.Validation;

namespace SolidApiExample.Application.People.DeletePerson;

public sealed class DeletePersonValidator : IRequestValidator<Guid>
{
    public ValidationResult Validate(Guid request) =>
        request == Guid.Empty
            ? ValidationResult.Failure("Id must be a non-empty GUID.")
            : ValidationResult.Success;
}
