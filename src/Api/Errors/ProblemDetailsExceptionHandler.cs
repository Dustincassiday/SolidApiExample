using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SolidApiExample.Application.Validation;

namespace SolidApiExample.Api.Errors;

/// <summary>
/// Converts known exceptions into RFC 7807 problem details responses.
/// </summary>
public sealed class ProblemDetailsExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ProblemDetailsExceptionHandler> _logger;

    public ProblemDetailsExceptionHandler(ILogger<ProblemDetailsExceptionHandler> logger) =>
        _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problem = exception switch
        {
            ValidationException validation => CreateValidationProblem(validation),
            KeyNotFoundException notFound => CreateProblemDetails(
                status: (int)HttpStatusCode.NotFound,
                title: "Resource not found",
                detail: notFound.Message),
            _ => CreateProblemDetails(
                status: (int)HttpStatusCode.InternalServerError,
                title: "An unexpected error occurred",
                detail: "If the problem persists contact support.")
        };

        if (problem.Status == (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception");
        }

        httpContext.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }

    private static ProblemDetails CreateValidationProblem(ValidationException exception)
    {
        var problem = CreateProblemDetails(
            status: (int)HttpStatusCode.BadRequest,
            title: "Validation failure",
            detail: "One or more validation errors occurred.");

        problem.Extensions["errors"] = exception.Errors;
        return problem;
    }

    private static ProblemDetails CreateProblemDetails(int status, string title, string detail) =>
        new()
        {
            Status = status,
            Title = title,
            Detail = detail
        };
}
