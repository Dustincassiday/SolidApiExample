using System;
using System.Collections.Generic;

namespace SolidApiExample.Application.Validation;

public sealed class ValidationException : Exception
{
    public ValidationException(IReadOnlyList<string> errors)
        : base("Validation failed.")
    {
        Errors = errors;
    }

    public IReadOnlyList<string> Errors { get; }
}
